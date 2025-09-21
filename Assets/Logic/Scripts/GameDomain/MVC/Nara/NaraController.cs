using Logic.Scripts.Services.AudioService;
using Logic.Scripts.Services.CommandFactory;
using Logic.Scripts.Services.ResourcesLoaderService;
using Logic.Scripts.Services.UpdateService;
using UnityEngine;

namespace Logic.Scripts.GameDomain.MVC.Nara {
    public class NaraController : INaraController, IFixedUpdatable {
        private readonly IUpdateSubscriptionService _updateSubscriptionService;
        private readonly IAudioService _audioService;
        private readonly ICommandFactory _commandFactory;
        private readonly IResourcesLoaderService _resourcesLoaderService;

        public GameObject NaraViewGO => _naraView.gameObject;

        private NaraView _naraView;
        private readonly NaraView _naraViewPrefab;
        private readonly NaraData _naraData;
        //private readonly NaraConfigurationSO _naraConfiguration;
        private readonly NaraMovementController _naraMovementController;

        public NaraController(IUpdateSubscriptionService updateSubscriptionService,
            IAudioService audioService, ICommandFactory commandFactory,
            IResourcesLoaderService resourcesLoaderService, NaraView naraViewPrefab,
            NaraConfigurationSO naraConfiguration) {
            _naraData = new NaraData(naraConfiguration);
            _updateSubscriptionService = updateSubscriptionService;
            _audioService = audioService;
            _commandFactory = commandFactory;
            _resourcesLoaderService = resourcesLoaderService;
            _naraViewPrefab = naraViewPrefab;
        }

        public void RegisterListeners() {
            _updateSubscriptionService.RegisterFixedUpdatable(this);
        }

        public void ManagedFixedUpdate() {

        }

        public void DisableCallbacks() {
            _naraView.RemoveAllCallbacks();
        }

        public void CreateNara() {
            _naraView = Object.Instantiate(_naraViewPrefab);
            _naraData.ResetData();
            _naraView.SetupCallbacks(OnNaraCollisionEnter, OnNaraTriggerEnter, OnNaraParticleCollisionEnter);
        }

        private void OnNaraCollisionEnter(Collision collision) {
            //Criar Comando Nara Colisão
        }

        private void OnNaraTriggerEnter(Collider collider) {
            //Criar Comando Nara Trigger
        }

        private void OnNaraParticleCollisionEnter(ParticleSystem particleSystem) {
            if (particleSystem.gameObject.TryGetComponent<AbilityView>(out AbilityView skillView)) {
                _commandFactory.CreateCommandVoid<SkillHitNaraCommand>().SetData(new SkillHitCommandData(skillView.AbilityData)).Execute();
            }
        }

        public void InitEntryPoint() {
            CreateNara();
        }

        public void ResetController() {

        }

        public void TakeDamage(int damageAmound) {
            _naraData.TakeDamage(damageAmound);
            //Invocar command de tomar dano
        }

        public void Heal(int healAmount) {
            _naraData.Heal(healAmount);
            //Invocar command de curar
        }

        public void AddShield(int value) {
            _naraData.AddShield(value);
            //Invocar command de receber escudo
        }
    }
}