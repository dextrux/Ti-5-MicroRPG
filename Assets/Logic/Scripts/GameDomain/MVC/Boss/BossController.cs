using Logic.Scripts.Services.AudioService;
using Logic.Scripts.Services.CommandFactory;
using Logic.Scripts.Services.ResourcesLoaderService;
using Logic.Scripts.Services.UpdateService;
using Logic.Scripts.GameDomain.MVC.Abilitys;
using UnityEngine;
using Zenject;
using System;
using System.Threading.Tasks;
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
        private readonly BossPhasesSO _bossPhases;
        private readonly IBossAbilityController _bossAbilityController;
        private ArenaPosReference _arenaReference;
        private BossBehaviorSO _activeBehavior;
        private int _currentPhaseIndex;

        private Rigidbody _bossRigidbody;
        private Transform _bossTransform;

        private enum BossMoveMode { None, Forward, TowardPlayer, Direction, Random }
        private BossMoveMode _moveMode;
        private Vector3 _fixedDirection;
        private float _moveSpeed;
        private float _rotationSpeed;
        private float _randomDirTimeRemaining;
        private float _randomDirDuration;
        private Vector3 _randomDirection;

        private float _turnMoveDistanceBudget;
        private int _executedTurnsCount;

        private BossPlan _currentPlan;
        private bool _isCasting;
        private int _remainingCastTurns;
        private struct PendingCast { public BossAttack Attack; public int TurnsRemaining; }
        private System.Collections.Generic.List<PendingCast> _pendingCasts;

        public bool IsCasting => _isCasting;
        public int RemainingCastTurns => _remainingCastTurns;

        public BossController(IUpdateSubscriptionService updateSubscriptionService,
            IAudioService audioService, ICommandFactory commandFactory,
            IResourcesLoaderService resourcesLoaderService, BossView bossViewPrefab,
            BossConfigurationSO bossConfiguration, BossPhasesSO bossPhases,
            IBossAbilityController bossAbilityController, IGamePlayUiController gamePlayUiController) {
            _updateSubscriptionService = updateSubscriptionService;
            _audioService = audioService;
            _commandFactory = commandFactory;
            _resourcesLoaderService = resourcesLoaderService;
            _bossViewPrefab = bossViewPrefab;
            _bossConfiguration = bossConfiguration;
            _bossPhases = bossPhases;
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
            _arenaReference = Object.FindFirstObjectByType<ArenaPosReference>();
            _gamePlayUiController.SetBossValues(_bossData.ActualHealth);
            if (_bossView != null) {
                _bossView.SetMoving(false);
            }
            _activeBehavior = GetBehaviorForPhaseIndex(0);
            _currentPhaseIndex = -1;
            _ = EvaluateAndMaybeSwitchPhaseAsync();
            if (_activeBehavior != null) {
                _bossAbilityController.SetBehavior(_activeBehavior);
            }
        }

        public void ManagedFixedUpdate() {
            if (_bossRigidbody == null || _bossTransform == null) return;
            if (_turnMoveDistanceBudget <= 0f) {
                if (_bossView != null) _bossView.SetMoving(false);
                return; // só mover durante o turno (quando há orçamento)
            }

            Vector3 worldDir = Vector3.zero;
            switch (_moveMode) {
                case BossMoveMode.Forward: {
                        worldDir = _bossTransform.forward;
                        break;
                    }
                case BossMoveMode.TowardPlayer: {
                        GameObject target = FindPlayerTarget();
                        if (target != null) {
                            Vector3 toPlayer = target.transform.position - _bossTransform.position;
                            toPlayer.y = 0f;
                            if (toPlayer.sqrMagnitude > 0.0001f) worldDir = toPlayer.normalized;
                        }
                        break;
                    }
                case BossMoveMode.Direction: {
                        Vector3 dir = _fixedDirection;
                        dir.y = 0f;
                        if (dir.sqrMagnitude > 0.0001f) worldDir = dir.normalized;
                        break;
                    }
                case BossMoveMode.Random: {
                        if (_randomDirTimeRemaining <= 0f || _randomDirection.sqrMagnitude < 0.0001f) {
                            float angle = UnityEngine.Random.Range(0f, 360f);
                            float rad = angle * Mathf.Deg2Rad;
                            _randomDirection = new Vector3(Mathf.Cos(rad), 0f, Mathf.Sin(rad));
                            _randomDirTimeRemaining = _randomDirDuration;
                        }
                        else {
                            _randomDirTimeRemaining -= Time.fixedDeltaTime;
                        }
                        worldDir = _randomDirection;
                        break;
                    }
                case BossMoveMode.None:
                default:
                    break;
            }

            if (worldDir.sqrMagnitude > 0.0001f) {
                if (_bossView != null) _bossView.SetMoving(true);
                float step = _moveSpeed * Time.fixedDeltaTime;
                if (step > _turnMoveDistanceBudget) step = _turnMoveDistanceBudget;
                _turnMoveDistanceBudget -= step;
                Vector3 fromPos = _bossTransform.position;
                Vector3 toPos = fromPos + worldDir.normalized * step;
                Vector3 toPlanar = new Vector3(toPos.x, fromPos.y, toPos.z);
                _bossRigidbody.MovePosition(toPlanar);

                Quaternion targetRot = Quaternion.LookRotation(worldDir.normalized, Vector3.up);
                Quaternion newRot = Quaternion.Slerp(_bossTransform.rotation, targetRot, Time.fixedDeltaTime * _rotationSpeed);
                _bossRigidbody.MoveRotation(newRot);
                if (_turnMoveDistanceBudget <= 0f) {
                    if (_bossView != null) _bossView.SetMoving(false);
                }
            }
        }

        public void DisableCallbacks() {
            _bossView.RemoveAllCallbacks();
        }

        public void CreateBoss() {
            _bossView = Object.Instantiate(_bossViewPrefab);
            _bossData.ResetData();
            _bossView.SetupCallbacks(PreviewHeal, PreviewDamage, TakeDamage, Heal);
            _bossRigidbody = _bossView != null ? _bossView.GetRigidbody() : null;
            _bossTransform = _bossView != null ? _bossView.transform : null;
            if (_bossView != null) {
                var relay = _bossView.gameObject.GetComponent<Assets.Logic.Scripts.GameDomain.Effects.EffectableRelay>();
                if (relay == null) relay = _bossView.gameObject.AddComponent<Assets.Logic.Scripts.GameDomain.Effects.EffectableRelay>();
                relay.Init(this);
            }

            if (_bossRigidbody != null) {
                _bossRigidbody.useGravity = false;
                _bossRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
                _bossRigidbody.interpolation = RigidbodyInterpolation.Interpolate;
                _bossRigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
            }

            _moveSpeed = _bossConfiguration != null ? _bossConfiguration.MoveSpeed : 5f;
            _rotationSpeed = _bossConfiguration != null ? _bossConfiguration.RotationSpeed : 5f;
            var behavior = GetCurrentBehavior();
            _randomDirDuration = behavior != null ? behavior.RandomChangeDirectionSeconds : 1.5f;
            _moveMode = BossMoveMode.TowardPlayer;
        }
        public void PlanNextTurn() { }

        public void ExecuteTurn() {
            // 1) Resolver pendências: decrementa e executa as que chegaram a 0
            ResolvePendingCasts();

            // 2) Definir movimento deste turno via padrão configurável
            ConfigureTurnMovement();

            _audioService.PlayAudio(AudioClipType.BossMove1SFX, AudioChannelType.Fx, AudioPlayType.OneShot);

            // 3) Preparar ataque deste turno conforme padrão do behavior
            QueuePreparedAttackFromBehavior();
            _executedTurnsCount++;
        }

        public async Task ExecuteTurnAsync() {
            ResolvePendingCasts();
            await ExecutePreparedActionAsync();
            // After executing the previously prepared action, evaluate phase change for this turn
            bool didTransition = await EvaluateAndMaybeSwitchPhaseAsync();
            await MoveTurnAsync();
            PrepareNextAction();
            _executedTurnsCount++;
        }

        private async Task ExecutePreparedActionAsync() {
            if (_pendingCasts == null || _pendingCasts.Count == 0) return;
            for (int i = _pendingCasts.Count - 1; i >= 0; i--) {
                if (_pendingCasts[i].TurnsRemaining <= 0) {
                    BossAttack castNow = _pendingCasts[i].Attack;
                    _pendingCasts.RemoveAt(i);
                    if (castNow != null) {
                        try {
                            await castNow.ExecuteAsync();
                        }
                        catch (Exception) { }
                    }
                }
            }
        }

        private async Task MoveTurnAsync() {
            ConfigureTurnMovement();
            if (_turnMoveDistanceBudget <= 0f) return;
            const float stopDistance = 1.2f;
            while (_turnMoveDistanceBudget > 0f) {
                if (_moveMode == BossMoveMode.TowardPlayer) {
                    GameObject target = FindPlayerTarget();
                    if (target != null && _bossTransform != null) {
                        Vector3 delta = target.transform.position - _bossTransform.position;
                        delta.y = 0f;
                        float dist = delta.magnitude;
                        if (dist <= stopDistance) {
                            _turnMoveDistanceBudget = 0f;
                            if (_bossView != null) _bossView.SetMoving(false);
                            break;
                        }
                    }
                }
                await Task.Yield();
            }
        }

        public void PlayPhaseTransitionAnimation() {
            _bossView?.PlayPhaseTransition();
        }

        private void PrepareNextAction() {
            QueuePreparedAttackFromBehavior();
        }

        private void QueuePreparedAttackFromBehavior() {
            var behavior = GetCurrentBehavior();
            if (behavior == null) return;
            BossBehaviorSO.BossTurnConfig[] pattern = behavior.TurnPattern;
            BossAttack[] pool = behavior.AvailableAttacks;
            if (pattern == null || pattern.Length == 0 || pool == null || pool.Length == 0) return;
            int indexInPattern = _executedTurnsCount % pattern.Length;
            BossBehaviorSO.BossTurnConfig entry = pattern[indexInPattern];
            int attackIndex = Mathf.Clamp(entry.AttackIndex, 0, pool.Length - 1);
            BossAttack attackInstance = _bossAbilityController?.CreateAttackAtIndex(attackIndex, _bossView.transform);
            if (attackInstance == null) {
                Debug.LogWarning("Boss attack instantiated: null");
                return;
            }
            if (_arenaReference != null) {
                attackInstance.Setup(_arenaReference, this);
            }
            Debug.Log($"Boss attack instantiated: {attackInstance.name} | index={attackIndex}");
            _pendingCasts.Add(new PendingCast { Attack = attackInstance, TurnsRemaining = 1 });
            Debug.Log($"Boss prepared attack index: {attackIndex} (executes next turn)");
        }

        private void ConfigureTurnMovement() {
            var behavior = GetCurrentBehavior();
            if (behavior == null) return;
            BossBehaviorSO.BossTurnConfig[] pattern = behavior.TurnPattern;
            float baseStep = behavior.StepDistance > 0f ? behavior.StepDistance : (behavior.FallbackStepDistance > 0f ? behavior.FallbackStepDistance : 1f);
            if (pattern == null || pattern.Length == 0) {
                SetMoveModeTowardPlayer(_bossConfiguration.MoveSpeed, _bossConfiguration.RotationSpeed);
                _turnMoveDistanceBudget = baseStep;
                Debug.Log($"Boss turn move: Mode=TowardPlayer | distance={_turnMoveDistanceBudget:0.###}");
                return;
            }
            int indexInPattern = _executedTurnsCount % pattern.Length;
            BossBehaviorSO.BossTurnConfig entry = pattern[indexInPattern];
            float multiplier = entry.DistanceMultiplier <= 0f ? 1f : entry.DistanceMultiplier;
            _turnMoveDistanceBudget = baseStep * multiplier;
            switch (entry.Mode) {
                case BossBehaviorSO.TurnMoveMode.Forward:
                    SetMoveModeForward(_bossConfiguration.MoveSpeed, _bossConfiguration.RotationSpeed);
                    Debug.Log($"Boss turn move: Mode=Forward | distance={_turnMoveDistanceBudget:0.###}");
                    break;
                case BossBehaviorSO.TurnMoveMode.TowardPlayer:
                    SetMoveModeTowardPlayer(_bossConfiguration.MoveSpeed, _bossConfiguration.RotationSpeed);
                    Debug.Log($"Boss turn move: Mode=TowardPlayer | distance={_turnMoveDistanceBudget:0.###}");
                    break;
                case BossBehaviorSO.TurnMoveMode.Direction:
                    SetMoveModeDirection(entry.Direction, _bossConfiguration.MoveSpeed, _bossConfiguration.RotationSpeed);
                    Debug.Log($"Boss turn move: Mode=Direction dir=({entry.Direction.x:0.###},{entry.Direction.y:0.###},{entry.Direction.z:0.###}) | distance={_turnMoveDistanceBudget:0.###}");
                    break;
                case BossBehaviorSO.TurnMoveMode.Random:
                    SetMoveModeRandom(_bossConfiguration.MoveSpeed, _bossConfiguration.RotationSpeed, behavior.RandomChangeDirectionSeconds);
                    Debug.Log($"Boss turn move: Mode=Random | distance={_turnMoveDistanceBudget:0.###}");
                    break;
            }
        }

        public void SetMoveModeForward(float moveSpeed, float rotationSpeed) {
            _moveSpeed = moveSpeed;
            _rotationSpeed = rotationSpeed;
            _moveMode = BossMoveMode.Forward;
        }

        public void SetMoveModeTowardPlayer(float moveSpeed, float rotationSpeed) {
            _moveSpeed = moveSpeed;
            _rotationSpeed = rotationSpeed;
            _moveMode = BossMoveMode.TowardPlayer;
        }

        public void SetMoveModeDirection(Vector3 worldDirection, float moveSpeed, float rotationSpeed) {
            _fixedDirection = worldDirection;
            _moveSpeed = moveSpeed;
            _rotationSpeed = rotationSpeed;
            _moveMode = BossMoveMode.Direction;
        }

        public void SetMoveModeRandom(float moveSpeed, float rotationSpeed, float changeDirectionSeconds = 1.5f) {
            _moveSpeed = moveSpeed;
            _rotationSpeed = rotationSpeed;
            _randomDirDuration = Mathf.Max(0.1f, changeDirectionSeconds);
            _randomDirTimeRemaining = 0f;
            _moveMode = BossMoveMode.Random;
        }



        private AbilityData SelectAbilityByHealth(out int selectedIndex) { selectedIndex = 0; return null; }
        private int GetAbilityDefaultDelay(AbilityData ability) { return 0; }

        private void ResolvePendingCasts() {
            if (_pendingCasts == null || _pendingCasts.Count == 0) return;
            for (int i = 0; i < _pendingCasts.Count; i++) {
                PendingCast pc = _pendingCasts[i];
                pc.TurnsRemaining -= 1;
                _pendingCasts[i] = pc;
            }
            // Do not execute here; execution happens in ExecutePreparedActionAsync
        }

        private GameObject FindPlayerTarget() {
            // Simplificação: procurar por NaraView na cena
            Nara.NaraView nara = Object.FindFirstObjectByType<Nara.NaraView>();
            return nara != null ? nara.gameObject : null;
        }





        public void TakeDamage(int amount) {
            _bossData.TakeDamage(amount);
            Debug.Log("Tomou dano!!!!!!!!!!!!!!!!");
            Debug.Log($"[Boss] Damage: -{amount} -> HP={_bossData.ActualHealth}/{_bossConfiguration.MaxHealth}");
            _gamePlayUiController.OnActualBossHealthChange(_bossData.ActualHealth);
            _gamePlayUiController.OnActualBossLifeChange(_bossData.ActualHealth);
            _gamePlayUiController.OnPreviewBossHealthChange(_bossData.ActualHealth);
            if (_bossData.ActualHealth <= 0) _gamePlayUiController.TempShowWinScreen();
        }

        public void Heal(int amount) {
            _bossData?.Heal(amount);
            Debug.Log($"[Boss] Heal: +{amount} -> HP={_bossData.ActualHealth}/{_bossConfiguration.MaxHealth}");
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

        public Transform GetReferenceTransform() {
            return _bossView.transform;
        }

        public void PreviewHeal(int healAmound) {

        }

        public void PreviewDamage(int damageAmound) {

        }

        public void ResetPreview() {

        }

        private BossBehaviorSO GetCurrentBehavior() {
            return _activeBehavior;
        }

        private async System.Threading.Tasks.Task<bool> EvaluateAndMaybeSwitchPhaseAsync() {
            if (_bossPhases == null || _bossConfiguration == null) return false;
            int newIndex = _bossPhases.GetPhaseIndexByHealth(_bossData.ActualHealth, _bossConfiguration.MaxHealth);
            if (newIndex == _currentPhaseIndex || newIndex < 0) return false;
            var phases = _bossPhases.Phases;
            string prevName = (_currentPhaseIndex >= 0 && phases != null && _currentPhaseIndex < phases.Length) ? phases[_currentPhaseIndex].Name : "(none)";
            BossBehaviorSO newBehavior = GetBehaviorForPhaseIndex(newIndex);
            string newName = (phases != null && newIndex < phases.Length) ? phases[newIndex].Name : "(unknown)";
            Debug.Log($"[Boss] Phase change: {_currentPhaseIndex}:{prevName} -> {newIndex}:{newName} at HP={_bossData.ActualHealth}/{_bossConfiguration.MaxHealth}");
            if (newBehavior != null && newBehavior != _activeBehavior) {
                _activeBehavior = newBehavior;
                _bossAbilityController.SetBehavior(_activeBehavior);
                _executedTurnsCount = 0;
                _pendingCasts?.Clear();
            } else if (newBehavior == null) {
                Debug.LogWarning($"[Boss] Phase '{newName}' has no Behavior assigned. Keeping current behavior.");
            }
            PlayPhaseTransitionAnimation();
            _currentPhaseIndex = newIndex;
            // wait for the phase transition animation before continuing turn
            if (_bossView != null) {
                float wait = Mathf.Max(0f, _bossView.GetPhaseTransitionDuration());
                if (wait > 0f) {
                    float elapsed = 0f;
                    while (elapsed < wait) {
                        elapsed += Time.deltaTime;
                        await System.Threading.Tasks.Task.Yield();
                    }
                }
            }
            return true;
        }

        private BossBehaviorSO GetBehaviorForPhaseIndex(int index)
        {
            if (_bossPhases == null) return null;
            var phases = _bossPhases.Phases;
            if (phases == null || index < 0 || index >= phases.Length) return null;
            return phases[index].Behavior;
        }

        public Transform GetTransformCastPoint() {
            return _bossView.GetTransformCastPoint();
        }

        public GameObject GetReferenceTargetPrefab() {
            return _bossView.GetReferenceTargetPrefab();
        }
    }
}
