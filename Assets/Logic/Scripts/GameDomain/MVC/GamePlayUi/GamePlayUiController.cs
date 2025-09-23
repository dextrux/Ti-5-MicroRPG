using Logic.Scripts.Core.Mvc.UICamera;
using Logic.Scripts.GameDomain.States;
using Logic.Scripts.Services.AudioService;
using Logic.Scripts.Services.StateMachineService;
using System.Threading;

namespace Logic.Scripts.GameDomain.MVC.Ui {
    public class GamePlayUiController: IGamePlayUiController {
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

        public void ShowGameOverPanel(int score, int scoreGoal, bool shouldShowScore, CancellationTokenSource cancellationTokenSource) {

        }

        public void UpdateScore(int newScore, CancellationTokenSource cancellationTokenSource) {

        }

        public void SwitchToInGameView() {

        }

        public void SwitchToBeforeGameView() {

        }

        public void SetStartingValues(int score, CancellationTokenSource cancellationTokenSource) {

        }

        public void ShowWinPanel(int winScore, CancellationTokenSource cancellationTokenSource) {

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

        public void OnSkill3CostChange(int newValue) => _gamePlayView.OnSkill3CostChange(newValue);


        public void OnSkill1NameChange(string newValue) => _gamePlayView.OnSkill1NameChange(newValue);

        public void OnSkill2NameChange(string newValue) => _gamePlayView.OnSkill2NameChange(newValue);

        public void OnSkill3NameChange(string newValue) => _gamePlayView.OnSkill3NameChange(newValue);

    }
}