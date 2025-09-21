using Logic.Scripts.Services.AudioService;
using Logic.Scripts.Services.CommandFactory;
using Logic.Scripts.Services.ResourcesLoaderService;
using Logic.Scripts.Services.UpdateService;
using Logic.Scripts.GameDomain.MVC.Abilitys;
using UnityEngine;
using Zenject;

namespace Logic.Scripts.GameDomain.MVC.Boss
{
    public class BossController : IBossController, IFixedUpdatable, IInitializable
    {
        private readonly IUpdateSubscriptionService _updateSubscriptionService;
        private readonly IAudioService _audioService;
        private readonly ICommandFactory _commandFactory;
        private readonly IResourcesLoaderService _resourcesLoaderService;

        private BossView _bossView;
        private readonly BossView _bossViewPrefab;
        private readonly BossData _bossData;
        private readonly BossConfigurationSO _bossConfiguration;

        private BossPlan _currentPlan;
        private bool _isCasting;
        private int _remainingCastTurns;

        public bool IsCasting => _isCasting;
        public int RemainingCastTurns => _remainingCastTurns;

        public BossController(IUpdateSubscriptionService updateSubscriptionService,
            IAudioService audioService, ICommandFactory commandFactory,
            IResourcesLoaderService resourcesLoaderService, BossView bossViewPrefab,
            BossConfigurationSO bossConfiguration)
        {
            _updateSubscriptionService = updateSubscriptionService;
            _audioService = audioService;
            _commandFactory = commandFactory;
            _resourcesLoaderService = resourcesLoaderService;
            _bossViewPrefab = bossViewPrefab;
            _bossConfiguration = bossConfiguration;
            _bossData = new BossData(_bossConfiguration);
        }

        public void Initialize()
        {
            _isCasting = false;
            _remainingCastTurns = 0;
            _updateSubscriptionService.RegisterFixedUpdatable(this);
            CreateBoss();
        }

        public void ManagedFixedUpdate() { }

        public void DisableCallbacks()
        {
            _bossView.RemoveAllCallbacks();
        }

        public void CreateBoss()
        {
            _bossView = Object.Instantiate(_bossViewPrefab);
            _bossData.ResetData();
            _bossView.SetupCallbacks(OnBossCollisionEnter, OnBossTriggerEnter, OnBossParticleCollisionEnter);
        }

        private void OnBossCollisionEnter(Collision collision) { }
        private void OnBossTriggerEnter(Collider collider) { }
        private void OnBossParticleCollisionEnter(ParticleSystem particleSystem) { }

        public void PlanNextTurn()
        {
            if (_isCasting && _currentPlan != null)
            {
                return;
            }

            _currentPlan = new BossPlan
            {
                MoveId = "StepForward",
                Ability = null,
                AbilityCastTurns = 0
            };
        }

        public void ExecuteTurn()
        {
            if (_isCasting)
            {
                _remainingCastTurns -= 1;
                if (_remainingCastTurns <= 0)
                {
                    if (_currentPlan != null && _currentPlan.Ability != null)
                    {
                        ExecuteAbility(_currentPlan.Ability);
                    }
                    _isCasting = false;
                }
                return;
            }

            if (_currentPlan != null)
            {
                ExecuteMove(_currentPlan.MoveId);

                if (_currentPlan.Ability != null)
                {
                    if (_currentPlan.AbilityCastTurns > 1)
                    {
                        _isCasting = true;
                        _remainingCastTurns = _currentPlan.AbilityCastTurns;
                    }
                    else
                    {
                        ExecuteAbility(_currentPlan.Ability);
                    }
                }
            }
        }

        private void ExecuteMove(string moveId)
        {
            BossMoveCommand cmd = _commandFactory.CreateCommandVoid<BossMoveCommand>();
            cmd.Execute();
        }

        private void ExecuteAbility(AbilityData ability)
        {
            BossCastAbilityCommand cmd = _commandFactory.CreateCommandVoid<BossCastAbilityCommand>();
            cmd.Execute();
        }
    }
}
