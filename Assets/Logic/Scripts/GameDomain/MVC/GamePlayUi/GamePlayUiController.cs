using Logic.Scripts.Core.Mvc.UICamera;
using Logic.Scripts.GameDomain.States;
using Logic.Scripts.Services.AudioService;
using Logic.Scripts.Services.StateMachineService;
using System.Threading;

namespace Logic.Scripts.GameDomain.MVC.Ui {
    public class GamePlayUiController {
        private readonly IStateMachineService _stateMachineService;
        private readonly LobbyState.Factory _lobbyStateFactory;
        private readonly IUICameraController _uiCameraController;
        private readonly IAudioService _audioService;
        private readonly GamePlayUiView _gamePlayView;
        private readonly GamePlayUiBindSO _gamePlayUiBindSO;

        public GamePlayUiController(IStateMachineService stateMachineService, LobbyState.Factory lobbyStateFactory, IUICameraController uiCameraController,
            GamePlayUiView gamePlayView, IAudioService audioService, GamePlayUiBindSO gamePlayUiBindSO) {
            _stateMachineService = stateMachineService;
            _lobbyStateFactory = lobbyStateFactory;
            _uiCameraController = uiCameraController;
            _gamePlayView = gamePlayView;
            _audioService = audioService;
            _gamePlayUiBindSO = gamePlayUiBindSO;
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
    }
}