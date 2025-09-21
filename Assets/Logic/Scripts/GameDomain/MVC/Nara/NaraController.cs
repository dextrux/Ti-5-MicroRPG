using Logic.Scripts.Services.AudioService;
using Logic.Scripts.Services.CommandFactory;
using Logic.Scripts.Services.ResourcesLoaderService;
using Logic.Scripts.Services.UpdateService;
using UnityEngine;

namespace Logic.Scripts.GameDomain.MVC.Nara {
    public class NaraController : INaraController, IFixedUpdatable, IEffectable {
        private readonly IUpdateSubscriptionService _updateSubscriptionService;
        private readonly IAudioService _audioService;
        private readonly ICommandFactory _commandFactory;
        private readonly IResourcesLoaderService _resourcesLoaderService;

        public GameObject NaraViewGO => _naraView.gameObject;
        public Transform NaraSkillSpotTransform => _naraView.SkillSpawnSpot;

        private NaraView _naraView;
        private readonly NaraView _naraViewPrefab;
        private readonly NaraData _naraData;
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




        private Vector3 GetMouseWorld() {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit)) {
                return hit.point;
            }

            return Camera.main.transform.forward * 60;
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

        public void TakeDamagePerTurn(int damageAmount, int duration) {

        }

        public void HealPerTurn(int healAmount, int duration) {

        }

        public void AddShieldPerTurn(int value, int duration) {

        }

        public void Stun(int value) {

        }

        public void SubtractActionPoints(int value) {

        }

        public void SubtractAllActionPoints(int value) {

        }

        public void ReduceActionPointsGain(int value) {

        }

        public void ReduceActionPointsGainPerTurn(int valueToSubtract, int duration) {

        }

        public void IncreaseActionPointsGainPerTurn(int valueToIncrease, int duration) {

        }

        public void AddActionPoints(int valueToIncrease) {

        }

        public void ReduceMovementPerTurn(int valueToSubtract, int duration) {

        }

        public void LimitActionPointUse(int value, int duration) {

        }
    }
}