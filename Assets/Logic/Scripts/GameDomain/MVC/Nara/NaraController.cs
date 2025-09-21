using Logic.Scripts.Services.AudioService;
using Logic.Scripts.Services.CommandFactory;
using Logic.Scripts.Services.ResourcesLoaderService;
using Logic.Scripts.Services.UpdateService;
using UnityEngine;

namespace Logic.Scripts.GameDomain.MVC.Nara {
    public class NaraController : INaraController, IFixedUpdatable
    {
        private readonly IUpdateSubscriptionService _updateSubscriptionService;
        private readonly IAudioService _audioService;
        private readonly ICommandFactory _commandFactory;
        private readonly IResourcesLoaderService _resourcesLoaderService;

        public GameObject NaraViewGO => _naraView.gameObject;
        public Transform NaraSkillSpotTransform => _naraView.SkillSpawnSpot;
        public NaraMovementController NaraMove => _naraMovementController;

        private NaraView _naraView;
        private readonly NaraView _naraViewPrefab;
        private readonly NaraData _naraData;
        private NaraMovementController _naraMovementController;

        private const float MoveSpeed = 10f;
        private const float RotationSpeed = 10f;

        private readonly global::GameInputActions _gameInputActions;

        public NaraController(IUpdateSubscriptionService updateSubscriptionService,
            IAudioService audioService, ICommandFactory commandFactory,
            IResourcesLoaderService resourcesLoaderService, NaraView naraViewPrefab,
            NaraConfigurationSO naraConfiguration)
        {
            _naraData = new NaraData(naraConfiguration);
            _updateSubscriptionService = updateSubscriptionService;
            _audioService = audioService;
            _commandFactory = commandFactory;
            _resourcesLoaderService = resourcesLoaderService;
            _naraViewPrefab = naraViewPrefab;
            _naraMovementController = new NaraMovementController(naraConfiguration);
            _gameInputActions = new global::GameInputActions();
            _gameInputActions.Enable();
        }

        public void RegisterListeners()
        {
            _updateSubscriptionService.RegisterFixedUpdatable(this);
        }

        public void ManagedFixedUpdate()
        {
            Vector2 dir = _gameInputActions.Player.Move.ReadValue<Vector2>();
            _naraMovementController.Move(dir, MoveSpeed, RotationSpeed);
        }

        public void DisableCallbacks()
        {
            _naraView.RemoveAllCallbacks();
        }

        public void CreateNara()
        {
            _naraView = Object.Instantiate(_naraViewPrefab);
            _naraData.ResetData();
            _naraView.SetupCallbacks(OnNaraCollisionEnter, OnNaraTriggerEnter, OnNaraParticleCollisionEnter);
            _naraMovementController.SetNaraRigidbody(_naraView.GetRigidbody());
            _naraMovementController.SetMovementRadiusCenter();
            _naraView.SetNaraCenterView(_naraMovementController.GetNaraCenter());
            _naraView.SetNaraRadiusView(_naraMovementController.GetNaraRadius());
        }

        private void OnNaraCollisionEnter(Collision collision)
        {
        }

        private void OnNaraTriggerEnter(Collider collider)
        {
        }

        private void OnNaraParticleCollisionEnter(ParticleSystem particleSystem)
        {
            if (particleSystem.gameObject.TryGetComponent<AbilityView>(out AbilityView skillView))
            {
                _commandFactory.CreateCommandVoid<SkillHitNaraCommand>().SetData(new SkillHitCommandData(skillView.AbilityData)).Execute();
            }
        }

        public void InitEntryPoint()
        {
            CreateNara();
        }

        public void ResetController()
        {
        }

        public void TakeDamage(int damageAmound)
        {
            _naraData.TakeDamage(damageAmound);
        }

        public void Heal(int healAmount)
        {
            _naraData.Heal(healAmount);
        }

        public void AddShield(int value)
        {
            _naraData.AddShield(value);
        }
    }
}
