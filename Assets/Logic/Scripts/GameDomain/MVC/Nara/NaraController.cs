using Logic.Scripts.GameDomain.MVC.Ui;
using Logic.Scripts.Services.AudioService;
using Logic.Scripts.Services.CommandFactory;
using Logic.Scripts.Services.ResourcesLoaderService;
using Logic.Scripts.Services.UpdateService;
using Logic.Scripts.Turns;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Logic.Scripts.GameDomain.MVC.Nara {
    public class NaraController : INaraController, IFixedUpdatable, IEffectable, IEffectableAction {
        private readonly IUpdateSubscriptionService _updateSubscriptionService;
        private readonly IAudioService _audioService;
        private readonly ICommandFactory _commandFactory;
        private readonly IResourcesLoaderService _resourcesLoaderService;
        private readonly IGamePlayUiController _gamePlayUiController;
        private readonly ITurnStateReader _turnStateReader;
        private readonly NaraView _naraViewPrefab;
        private readonly NaraData _naraData;
        private readonly NaraConfigurationSO _naraConfiguration;
        public GameObject NaraViewGO => _naraView.gameObject;
        public Transform NaraSkillSpotTransform => _naraView.transform;
        public NaraMovementController NaraMove => _naraMovementController;

        private NaraView _naraView;
        private NaraMovementController _naraMovementController;
        private int _debuffStacks;

        private readonly global::GameInputActions _gameInputActions;

        //Teste debuffs
        private List<StatusSO> debuffs;

        public NaraController(IUpdateSubscriptionService updateSubscriptionService,
            IAudioService audioService, ICommandFactory commandFactory,
            IResourcesLoaderService resourcesLoaderService, IGamePlayUiController gamePlayUiController, NaraView naraViewPrefab,
            NaraConfigurationSO naraConfiguration, global::GameInputActions inputActions, ITurnStateReader turnStateReader) {
            _naraData = new NaraData(naraConfiguration);
            _naraConfiguration = naraConfiguration;
            _updateSubscriptionService = updateSubscriptionService;
            _audioService = audioService;
            _commandFactory = commandFactory;
            _resourcesLoaderService = resourcesLoaderService;
            _naraViewPrefab = naraViewPrefab;
            _gamePlayUiController = gamePlayUiController;
            _gameInputActions = new global::GameInputActions();
            _gameInputActions.Enable();
            _turnStateReader = turnStateReader;
        }

        public void RegisterListeners() {
            _updateSubscriptionService.RegisterFixedUpdatable(this);
        }

        public void UnregisterListeners() {
            _updateSubscriptionService.UnregisterFixedUpdatable(this);
        }

        public void ManagedFixedUpdate() {
            if (_naraMovementController is NaraTurnMovementController) {
                if (_turnStateReader != null && _turnStateReader.Active && _turnStateReader.Phase == TurnPhase.PlayerAct) {
                    Vector2 dir = _gameInputActions.Player.Move.ReadValue<Vector2>();
                    _naraMovementController.Move(dir, _naraConfiguration.MoveSpeed, _naraConfiguration.RotationSpeed);
                    bool willMove = dir.sqrMagnitude > 0.0001f && _naraConfiguration.MoveSpeed > 0f;
                    _naraView?.SetMoving(willMove);
                }
                else {
                    _naraMovementController.Move(Vector2.zero, 0f, 0f);
                    _naraView?.SetMoving(false);
                }
            }
            else {
                Vector2 dir = _gameInputActions.Player.Move.ReadValue<Vector2>();
                if (dir != Vector2.zero) {
                    _naraMovementController.Move(dir, _naraConfiguration.MoveSpeed, _naraConfiguration.RotationSpeed);
                    bool willMove = dir.sqrMagnitude > 0.0001f && _naraConfiguration.MoveSpeed > 0f;
                    _naraView?.SetMoving(willMove);
                }
                else {
                    _naraMovementController.Move(Vector2.zero, 0f, 0f);
                    _naraView?.SetMoving(false);
                }
            }
        }

        public void CreateNara(NaraMovementController movementController) {
            _naraView = Object.Instantiate(_naraViewPrefab);
            _naraData.ResetData();
            _naraView.SetMoving(false);
            _naraMovementController = movementController;
        }

        public void ResetController() {
            UnregisterListeners();
            _naraData.ResetData();
            UnityEngine.Object.Destroy(_naraView.gameObject);
        }

        public void InitEntryPoint() {
            _gamePlayUiController.SetPlayerValues(_naraData.ActualHealth, _naraData.PreviewHealth);
            _naraMovementController.InitEntryPoint(_naraView.GetRigidbody(), _naraView.GetCamera());
        }

        public void SetPosition(Vector3 movementCenter) {
            _naraView.GetRigidbody().position = movementCenter;
        }

        //public void RecenterMovementAreaAtTurnStart() {
        //    _naraMovementController.SetMovementRadiusCenter();
        //    _naraView.SetNaraMovementAreaAgain(_naraMovementController.GetNaraRadius(), _naraMovementController.GetNaraCenter());
        //}

        //public void SetMovementCircleVisible(bool visible) {
        //    if (_naraView != null) {
        //        _naraView.SetMovementCircleVisible(visible);
        //    }
        //}

        //public void SetNewMovementArea() {
        //    _naraMovementController.RecalculateRadiusAfterAbility();
        //    RecenterMovementAreaAtTurnStart();
        //}

        //public void ResetMovementArea() {
        //    _naraMovementController.ResetMovementRadius();
        //    RecenterMovementAreaAtTurnStart();
        //}

        //public void RemoveMovementAreaLimit() {
        //    _naraMovementController.RemoveMovementRadius();
        //    RecenterMovementAreaAtTurnStart();
        //}

        //public void RecenterNara() {
        //    _naraView.SetPosition();
        //}

        //Funcao pra setar o raio para zero. Isso zera a movimentacao
        //To-Do levar isso para o lugar correto
        //public void CancelMovement() {
        //    _naraMovementController.SetRadiusToZero();
        //    _naraView.SetNaraRadiusView(0);
        //}

        #region IEffectable Methods

        public Transform GetReferenceTransform() {
            return _naraView.transform;
        }

        public void ResetPreview() {
            _naraData.ResetPreview();
        }
        public void PreviewDamage(int damageAmound) {
            _naraData.TakeDamage(damageAmound);
            _gamePlayUiController.OnPreviewPlayerLifePercentChange(_naraData.ActualHealth);
        }

        public void PreviewHeal(int damageAmound) {
            _naraData.TakeDamage(damageAmound);
            _gamePlayUiController.OnPreviewPlayerLifePercentChange(_naraData.ActualHealth);
        }

        public void TakeDamage(int damageAmound) {
            _naraData.TakeDamage(damageAmound);
            _gamePlayUiController.OnActualPlayerHealthChange(_naraData.ActualHealth);
            _gamePlayUiController.OnActualPlayerLifePercentChange(_naraData.ActualHealth);
            _gamePlayUiController.OnPreviewPlayerLifePercentChange(_naraData.ActualHealth);
            if (_naraData.IsAlive()) {
                _naraView?.PlayDeath();
            }
        }

        public void PlayAttackType(int type) {
            _naraView?.SetAttackType(type);
        }

        public void PlayAttackType1() {
            _naraView?.SetAttackType(1);
        }

        public void Heal(int healAmount) {
            _naraData.Heal(healAmount);
            _gamePlayUiController.OnActualPlayerHealthChange(_naraData.ActualHealth);
            _gamePlayUiController.OnActualPlayerLifePercentChange(_naraData.ActualHealth);
        }

        public void TriggerExecute() {
            _naraView?.TriggerExecute();
        }

        public void ResetExecuteTrigger() {
            _naraView?.ResetExecuteTrigger();
        }

		public void TriggerCancel() {
			_naraView?.TriggerCancel();
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
        public int GetNumberDebuffs() {
            return _debuffStacks;
        }

        public void AddDebuffStacks(int amount) {
            if (amount <= 0) return;
            _debuffStacks += amount;
            Debug.Log($"Nara debuff stacks updated: {_debuffStacks} (+{amount})");
        }

        public int GetDebuffStacks() {
            return _debuffStacks;
        }

        public Transform GetTransformCastPoint() {
            return _naraView.CastPoint;
        }

        public GameObject GetReferenceTargetPrefab() {
            return _naraView.TargetPrefab;
        }

        public LineRenderer GetPointLineRenderer() {
            return _naraView.CastLineRenderer;
        }
    }
}
