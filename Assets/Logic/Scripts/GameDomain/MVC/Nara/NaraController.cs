using Logic.Scripts.GameDomain.MVC.Ui;
using Logic.Scripts.Services.AudioService;
using Logic.Scripts.Services.CommandFactory;
using Logic.Scripts.Services.ResourcesLoaderService;
using Logic.Scripts.Services.UpdateService;
using Logic.Scripts.GameDomain.MVC.Abilitys;
using UnityEngine;
using Logic.Scripts.Turns;
using System.Collections.Generic;

namespace Logic.Scripts.GameDomain.MVC.Nara {
    public class NaraController : INaraController, IFixedUpdatable, IEffectable, IEffectableAction
    {
        private readonly IUpdateSubscriptionService _updateSubscriptionService;
        private readonly IAudioService _audioService;
        private readonly ICommandFactory _commandFactory;
        private readonly IResourcesLoaderService _resourcesLoaderService;
        private readonly IGamePlayUiController _gamePlayUiController;
        private readonly ITurnStateReader _turnStateReader;
        //int i = 0; Nao usado
        public GameObject NaraViewGO => _naraView.gameObject;
        public Transform NaraSkillSpotTransform => _naraView.transform;
        public NaraMovementController NaraMove => _naraMovementController;

        private NaraView _naraView;
        private readonly NaraView _naraViewPrefab;
        private readonly NaraData _naraData;
        private NaraMovementController _naraMovementController;
        private int _debuffStacks;

        private const float MoveSpeed = 15f;
        private const float RotationSpeed = 10f;

        private readonly global::GameInputActions _gameInputActions;

        //Teste debuffs
        private List<StatusSO> debuffs;

        public NaraController(IUpdateSubscriptionService updateSubscriptionService,
            IAudioService audioService, ICommandFactory commandFactory,
            IResourcesLoaderService resourcesLoaderService, IGamePlayUiController gamePlayUiController, NaraView naraViewPrefab,
            NaraConfigurationSO naraConfiguration, global::GameInputActions inputActions, ITurnStateReader turnStateReader)
        {
            _naraData = new NaraData(naraConfiguration);
            _updateSubscriptionService = updateSubscriptionService;
            _audioService = audioService;
            _commandFactory = commandFactory;
            _resourcesLoaderService = resourcesLoaderService;
            _naraViewPrefab = naraViewPrefab;
            _gamePlayUiController = gamePlayUiController;
            _naraMovementController = new NaraMovementController(naraConfiguration, inputActions, updateSubscriptionService);
            _gameInputActions = new global::GameInputActions();
            _gameInputActions.Enable();
            _turnStateReader = turnStateReader;
        }

        public void RegisterListeners()
        {
            _updateSubscriptionService.RegisterFixedUpdatable(this);
        }

        public void ManagedFixedUpdate()
        {
            if (_turnStateReader != null && _turnStateReader.Active && _turnStateReader.Phase == TurnPhase.PlayerAct)
            {
                Vector2 dir = _gameInputActions.Player.Move.ReadValue<Vector2>();
                _naraMovementController.Move(dir, MoveSpeed, RotationSpeed);
            }
            else
            {
                _naraMovementController.Move(Vector2.zero, 0f, 0f);
            }
        }

        public void CreateNara()
        {
            _naraView = Object.Instantiate(_naraViewPrefab);
            _naraData.ResetData();
            _naraMovementController.SetNaraRigidbody(_naraView.GetRigidbody());
            _naraMovementController.SetMovementRadiusCenter();
            _naraView.SetNaraCenterView(_naraMovementController.GetNaraCenter());
            _naraView.SetNaraRadiusView(_naraMovementController.GetNaraRadius());
            _naraView.CreateLineRenderer();
            _naraView.SetCamera();
            _naraMovementController.SetCamera(_naraView.GetCamera());
        }

        public void ExecuteAbility(AbilityData abilityData, IEffectable castter)
        {
            _commandFactory.CreateCommandVoid<SkillHitNaraCommand>().SetData(new SkillHitCommandData(abilityData, castter, this)).Execute();
        }

        public void InitEntryPoint()
        {
            CreateNara();
            _gamePlayUiController.SetPlayerValues(_naraData.ActualHealth, _naraData.PreviewHealth);
        }

        public void ResetController()
        {
        }

        public void RecenterMovementAreaAtTurnStart()
        {
            _naraMovementController.SetMovementRadiusCenter();
            _naraView.SetNaraMovementAreaAgain(_naraMovementController.GetNaraRadius(), _naraMovementController.GetNaraCenter());
        }

        public void SetMovementCircleVisible(bool visible)
        {
            if (_naraView != null)
            {
                _naraView.SetMovementCircleVisible(visible);
            }
        }

        #region IEffectable Methods

        public Transform GetReferenceTransform() {
            return _naraView.transform;
        }

        public void ResetPreview() {
            _naraData.ResetPreview();
        }
        public void PreviewDamage(int damageAmound)
        {
            _naraData.TakeDamage(damageAmound);
            _gamePlayUiController.OnPreviewPlayerLifePercentChange(_naraData.ActualHealth);
        }

        public void PreviewHeal(int damageAmound)
        {
            _naraData.TakeDamage(damageAmound);
            _gamePlayUiController.OnPreviewPlayerLifePercentChange(_naraData.ActualHealth);
        }

        public void TakeDamage(int damageAmound)
        {
            _naraData.TakeDamage(damageAmound);
            _gamePlayUiController.OnActualPlayerHealthChange(_naraData.ActualHealth);
            _gamePlayUiController.OnActualPlayerLifePercentChange(_naraData.ActualHealth);
            _gamePlayUiController.OnPreviewPlayerLifePercentChange(_naraData.ActualHealth);
            if (_naraData.IsAlive()) _gamePlayUiController.TempShowLoseScreen();
        }

        public void Heal(int healAmount)
        {
            _naraData.Heal(healAmount);
            _gamePlayUiController.OnActualPlayerHealthChange(_naraData.ActualHealth);
            _gamePlayUiController.OnActualPlayerLifePercentChange(_naraData.ActualHealth);
        }

        public void TakeDamagePerTurn(int damageAmount, int duration) {

        }

        public void HealPerTurn(int healAmount, int duration) {

        }

        public void Stun(int value) {

        }

        public void SubtractActionPoints(int value) {

        }

        public void SubtractAllActionPoints(int value) {

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
        #endregion

        // Debuff API
        public void AddDebuffStacks(int amount)
        {
            if (amount <= 0) return;
            _debuffStacks += amount;
            Debug.Log($"Nara debuff stacks updated: {_debuffStacks} (+{amount})");
        }

        public int GetDebuffStacks()
        {
            return _debuffStacks;
        }
    }
}
