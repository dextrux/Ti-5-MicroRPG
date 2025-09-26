using Logic.Scripts.Services.AudioService;
using Logic.Scripts.Services.CommandFactory;
using Logic.Scripts.Services.ResourcesLoaderService;
using Logic.Scripts.Services.UpdateService;
using Logic.Scripts.GameDomain.MVC.Abilitys;
using UnityEngine;
using Zenject;
using System;
using Object = UnityEngine.Object;
using Logic.Scripts.GameDomain.MVC.Ui;

namespace Logic.Scripts.GameDomain.MVC.Boss {
    public class BossController : IBossController, IFixedUpdatable, IInitializable, IEffectable {
        private readonly IUpdateSubscriptionService _updateSubscriptionService;
        private readonly IAudioService _audioService;
        private readonly ICommandFactory _commandFactory;
        private readonly IResourcesLoaderService _resourcesLoaderService;
        private readonly IGamePlayUiController _gamePlayUiController;

        private BossView _bossView;
        private readonly BossView _bossViewPrefab;
        private readonly BossData _bossData;
        private readonly BossConfigurationSO _bossConfiguration;
        private readonly BossBehaviorSO _bossBehavior;
        private readonly IBossAbilityController _bossAbilityController;

        private BossPlan _currentPlan;
        private bool _isCasting;
        private int _remainingCastTurns;
        private struct PendingCast { public AbilityData Ability; public int TurnsRemaining; }
        private System.Collections.Generic.List<PendingCast> _pendingCasts;

        public bool IsCasting => _isCasting;
        public int RemainingCastTurns => _remainingCastTurns;

        public BossController(IUpdateSubscriptionService updateSubscriptionService,
            IAudioService audioService, ICommandFactory commandFactory,
            IResourcesLoaderService resourcesLoaderService, BossView bossViewPrefab,
            BossConfigurationSO bossConfiguration, BossBehaviorSO bossBehavior,
            IBossAbilityController bossAbilityController, IGamePlayUiController gamePlayUiController) {
            _updateSubscriptionService = updateSubscriptionService;
            _audioService = audioService;
            _commandFactory = commandFactory;
            _resourcesLoaderService = resourcesLoaderService;
            _bossViewPrefab = bossViewPrefab;
            _bossConfiguration = bossConfiguration;
            _bossBehavior = bossBehavior;
            _bossAbilityController = bossAbilityController;
            _gamePlayUiController = gamePlayUiController;
            _bossData = new BossData(_bossConfiguration);
        }

        public void Initialize() {
            _isCasting = false;
            _remainingCastTurns = 0;
            _pendingCasts = new System.Collections.Generic.List<PendingCast>();
            _updateSubscriptionService.RegisterFixedUpdatable(this);
            CreateBoss();
        }

        public void ManagedFixedUpdate() { }

        public void DisableCallbacks() {
            _bossView.RemoveAllCallbacks();
        }

        public void CreateBoss() {
            _bossView = Object.Instantiate(_bossViewPrefab);
            _bossData.ResetData();
            _bossView.SetupCallbacks(OnBossCollisionEnter, OnBossTriggerEnter, OnBossParticleCollisionEnter);
        }

        private void OnBossCollisionEnter(Collision collision) { }
        private void OnBossTriggerEnter(Collider collider) { }
        private void OnBossParticleCollisionEnter(ParticleSystem particleSystem) {
            if (particleSystem.gameObject.TryGetComponent<AbilityView>(out AbilityView skillView)) {
                _commandFactory.CreateCommandVoid<SkillHitNaraCommand>().SetData(new SkillHitCommandData(skillView.AbilityData, this, this)).Execute();
            }
        }

        public void PlanNextTurn() {
            if (_isCasting && _currentPlan != null) {
                return;
            }

            int tmpIndex;
            AbilityData ability = SelectAbilityByHealth(out tmpIndex);

            _currentPlan = new BossPlan {
                MoveId = "StepForward",
                Ability = ability,
                AbilityCastTurns = 1
            };
        }

        public void ExecuteTurn() {
            // 1) Resolver pendências: decrementa e executa as que chegaram a 0
            ResolvePendingCasts();

            // 2) Movimento deste turno
            if (_currentPlan != null) {
                ExecuteMove(_currentPlan.MoveId);
            }

            // 3) Novo cast deste turno: escolher ability e enfileirar com seu delay
            int abilityIndex;
            AbilityData abilityNow = SelectAbilityByHealth(out abilityIndex);
            if (abilityNow != null) {
                int delay = GetAbilityDefaultDelay(abilityNow);
                if (delay <= 0) {
                    ExecuteAbility(abilityNow, abilityIndex);
                }
                else {
                    _pendingCasts.Add(new PendingCast { Ability = abilityNow, TurnsRemaining = delay });
                    Debug.Log($"Boss queued ability: {abilityNow.name} with delay {delay}");
                }
            }
        }

        private void ExecuteMove(string moveId) {
            BossMoveCommand cmd = _commandFactory.CreateCommandVoid<BossMoveCommand>();
            Vector3 dir;
            float dist;
            BossBehaviorSO.MoveKind moveKind;
            ComputeMove(out dir, out dist, out moveKind);
            cmd.SetStep(dir, dist);
            cmd.Execute();
            Debug.Log($"Boss move executed: {moveKind} | stepDistance={dist}");
        }

        private void ExecuteAbility(AbilityData ability, int abilityIndex = 0) {
            if (ability == null) return;
            // 1) Spawn a visual AbilityView do Boss
            _bossAbilityController?.CreateAbilityAtIndex(abilityIndex, _bossView.transform);
            // 2) Executa efeitos lógicos (enquanto a View for apenas visual)
            BossCastAbilityCommand cmd = _commandFactory.CreateCommandVoid<BossCastAbilityCommand>();
            GameObject caster = _bossView.gameObject;
            GameObject target = FindPlayerTarget();
            cmd.SetContext(ability, caster, target);
            cmd.Execute();
            Debug.Log($"Boss ability cast: {ability.Name}");
        }

        private AbilityData SelectAbilityByHealth(out int selectedIndex) {
            selectedIndex = 0;
            if (_bossBehavior == null) return null;
            float hpRatio = _bossData.ActualHealth <= 0 ? 0f : (float)_bossData.ActualHealth / _bossConfiguration.MaxHealth;
            AbilityData[] pool = hpRatio > 0.5f ? _bossBehavior.HighHpAbilities : _bossBehavior.LowHpAbilities;
            if (pool == null || pool.Length == 0) return null;
            selectedIndex = UnityEngine.Random.Range(0, pool.Length);
            return pool[selectedIndex];
        }

        private int GetAbilityDefaultDelay(AbilityData ability) {
            if (_bossBehavior == null || ability == null) return 0;
            float hpRatio = _bossData.ActualHealth <= 0 ? 0f : (float)_bossData.ActualHealth / _bossConfiguration.MaxHealth;
            AbilityData[] pool = hpRatio > 0.5f ? _bossBehavior.HighHpAbilities : _bossBehavior.LowHpAbilities;
            int[] delays = hpRatio > 0.5f ? _bossBehavior.HighHpDelays : _bossBehavior.LowHpDelays;
            if (pool == null || delays == null) return 0;
            for (int i = 0; i < pool.Length && i < delays.Length; i++) {
                if (pool[i] == ability) return delays[i];
            }
            return 0;
        }

        private void ResolvePendingCasts() {
            if (_pendingCasts == null || _pendingCasts.Count == 0) return;
            for (int i = 0; i < _pendingCasts.Count; i++) {
                PendingCast pc = _pendingCasts[i];
                pc.TurnsRemaining -= 1;
                _pendingCasts[i] = pc;
            }
            for (int i = _pendingCasts.Count - 1; i >= 0; i--) {
                if (_pendingCasts[i].TurnsRemaining <= 0) {
                    AbilityData castNow = _pendingCasts[i].Ability;
                    ExecuteAbility(castNow);
                    _pendingCasts.RemoveAt(i);
                }
            }
        }

        private GameObject FindPlayerTarget() {
            // Simplificação: procurar por NaraView na cena
            Nara.NaraView nara = Object.FindObjectOfType<Nara.NaraView>();
            return nara != null ? nara.gameObject : null;
        }

        private void ComputeMove(out Vector3 direction, out float distance, out BossBehaviorSO.MoveKind moveKind) {
            direction = _bossView.transform.forward;
            distance = _bossBehavior != null ? _bossBehavior.StepDistance : 1f;
            moveKind = BossBehaviorSO.MoveKind.Forward;
            BossBehaviorSO.BossMoveEntry[] pattern = _bossBehavior != null ? _bossBehavior.MovePattern : null;
            if (pattern == null || pattern.Length == 0) return;
            int index = (_pendingCasts != null ? _pendingCasts.Count : 0) % pattern.Length; // simple cycling
            BossBehaviorSO.BossMoveEntry entry = pattern[index];
            moveKind = entry.Kind;
            if (entry.Distance > 0) distance = entry.Distance;
            switch (entry.Kind) {
                case BossBehaviorSO.MoveKind.Forward:
                    direction = _bossView.transform.forward;
                    break;
                case BossBehaviorSO.MoveKind.TowardPlayer: {
                        GameObject target = FindPlayerTarget();
                        if (target != null) {
                            Vector3 toPlayer = target.transform.position - _bossView.transform.position;
                            toPlayer.y = 0f;
                            if (toPlayer.sqrMagnitude > 0.0001f) direction = toPlayer.normalized;
                        }
                        break;
                    }
                case BossBehaviorSO.MoveKind.Offset:
                    direction = entry.Offset.sqrMagnitude > 0.0001f ? entry.Offset.normalized : _bossView.transform.forward;
                    break;
            }
        }



        public void TakeDamage(int amount) {
            _bossData?.TakeDamage(amount);
            _gamePlayUiController.OnActualBossHealthChange(_bossData.ActualHealth);
            _gamePlayUiController.OnActualBossLifeChange(_bossData.ActualHealth);
            _gamePlayUiController.OnPreviewBossHealthChange(_bossData.ActualHealth);
        }

        public void Heal(int amount) {
            _bossData?.Heal(amount);
            _gamePlayUiController.OnActualBossHealthChange(_bossData.ActualHealth);
            _gamePlayUiController.OnActualBossLifeChange(_bossData.ActualHealth);
            _gamePlayUiController.OnPreviewBossHealthChange(_bossData.ActualHealth);
        }

        public void AddShield(int amount) {
            _bossData?.AddShield(amount);
        }

        public bool IsAlive() {
            return _bossData != null && _bossData.IsAlive();
        }

        public void TakeDamagePerTurn(int damageAmount, int duration) {
            throw new NotImplementedException();
        }

        public void HealPerTurn(int healAmount, int duration) {
            throw new NotImplementedException();
        }

        public void AddShieldPerTurn(int value, int duration) {
            throw new NotImplementedException();
        }

        public void Stun(int value) {
            throw new NotImplementedException();
        }

        public void SubtractActionPoints(int value) {
            throw new NotImplementedException();
        }

        public void SubtractAllActionPoints(int value) {
            throw new NotImplementedException();
        }

        public void ReduceActionPointsGain(int value) {
            throw new NotImplementedException();
        }

        public void ReduceActionPointsGainPerTurn(int valueToSubtract, int duration) {
            throw new NotImplementedException();
        }

        public void IncreaseActionPointsGainPerTurn(int valueToIncrease, int duration) {
            throw new NotImplementedException();
        }

        public void AddActionPoints(int valueToIncrease) {
            throw new NotImplementedException();
        }

        public void ReduceMovementPerTurn(int valueToSubtract, int duration) {
            throw new NotImplementedException();
        }

        public void LimitActionPointUse(int value, int duration) {
            throw new NotImplementedException();
        }
    }
}
