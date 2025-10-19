using Logic.Scripts.Core.Mvc.UICamera;
using Logic.Scripts.GameDomain.States;
using Logic.Scripts.Services.AudioService;
using Logic.Scripts.Services.StateMachineService;
using System.Diagnostics;
using System.Threading;
using UnityEngine;

namespace Logic.Scripts.GameDomain.MVC.Ui {
    public class GamePlayUiController : IGamePlayUiController {
        private readonly IStateMachineService _stateMachineService;
        private readonly LobbyState.Factory _lobbyStateFactory;
        private readonly IUICameraController _uiCameraController;
        private readonly IAudioService _audioService;
        private readonly GamePlayUiView _gamePlayView;
        private readonly GamePlayUiBindSO _gamePlayUiBindSO;

        public GamePlayUiController(IStateMachineService stateMachineService, LobbyState.Factory lobbyStateFactory, IUICameraController uiCameraController,
            GamePlayUiView gamePlayView, IAudioService audioService) {
            _stateMachineService = stateMachineService;
            _lobbyStateFactory = lobbyStateFactory;
            _uiCameraController = uiCameraController;
            _gamePlayView = gamePlayView;
            _audioService = audioService;
        }

        public void TempHoldScreenHide() {
            _gamePlayView.TempHoldScreenHide();
        }

        public void TempShowWinScreen() {
            _gamePlayView.TempShowWinScreen();
        }

        public void TempShowLoseScreen() {
            _gamePlayView.TempShowLoseScreen();
        }

        public void SwitchToInGameView() {

        }

        public void SwitchToBeforeGameView() {

        }

        public void SetBossValues(int newValue) {
            _gamePlayView.OnActualBossHealthChange(newValue);

            _gamePlayView.OnPreviewBossHealthChange(newValue);

            _gamePlayView.OnActualBossLifeChange(newValue);
        }

        public void SetBossValues(int newPreviewValue, int newActualValue) {
            _gamePlayView.OnActualBossHealthChange(newActualValue);

            _gamePlayView.OnPreviewBossHealthChange(newPreviewValue);

            _gamePlayView.OnActualBossLifeChange(newActualValue);
        }

        public void SetPlayerValues(int newValue) {
            _gamePlayView.OnActualPlayerLifePercentChange(newValue);

            _gamePlayView.OnPreviewPlayerLifePercentChange(newValue);

            _gamePlayView.OnActualPlayerHealthChange(newValue);
        }

        public void SetPlayerValues(int newPreviewValue, int newActualValue) {
            _gamePlayView.OnActualPlayerLifePercentChange(newActualValue);

            _gamePlayView.OnPreviewPlayerLifePercentChange(newPreviewValue);

            _gamePlayView.OnActualPlayerHealthChange(newActualValue);
        }

        public void SetAbilityValues(int ability1Cost, string ability1Name, 
            int ability2Cost, string ability2Name) {
            UnityEngine.Debug.Log("GameplayView: ");
            _gamePlayView.OnSkill1CostChange(ability1Cost);

            _gamePlayView.OnSkill2CostChange(ability2Cost);

            _gamePlayView.OnSkill1NameChange(ability1Name);

            _gamePlayView.OnSkill2NameChange(ability2Name);
        }

        public void ShowWinPanel(CancellationTokenSource cancellationTokenSource) {

        }

        public void ShowGameOverPanel(CancellationTokenSource cancellationTokenSource) {

        }

        public void InitExitPoint() {

        }


        public void OnActualBossHealthChange(int newValue) => _gamePlayView.OnActualBossHealthChange(newValue);

        public void OnPreviewBossHealthChange(int newValue) => _gamePlayView.OnPreviewBossHealthChange(newValue);

        public void OnActualBossLifeChange(int newValue) => _gamePlayView.OnActualBossLifeChange(newValue);

        public void OnActualPlayerLifePercentChange(int newValue) => _gamePlayView.OnActualPlayerLifePercentChange(newValue);

        public void OnPreviewPlayerLifePercentChange(int newValue) => _gamePlayView.OnPreviewPlayerLifePercentChange(newValue);

        public void OnActualPlayerHealthChange(int newValue) => _gamePlayView.OnActualPlayerHealthChange(newValue);

        public void OnPlayerActionPointsChange(int newValue) => _gamePlayView.OnPlayerActionPointsChange(newValue);

        public void OnSkill1CostChange(int newValue) => _gamePlayView.OnSkill1CostChange(newValue);

        public void OnSkill2CostChange(int newValue) => _gamePlayView.OnSkill2CostChange(newValue);

        public void OnSkill1NameChange(string newValue) => _gamePlayView.OnSkill1NameChange(newValue);

        public void OnSkill2NameChange(string newValue) => _gamePlayView.OnSkill2NameChange(newValue);

    }
}