using Logic.Scripts.Core.Mvc.WorldCamera;
using Logic.Scripts.GameDomain.GameInputActions;
using Logic.Scripts.GameDomain.MVC.Nara;
using Logic.Scripts.Services.AudioService;
using Logic.Scripts.Services.CommandFactory;
using Logic.Scripts.Services.UpdateService;
using System.Threading;
using UnityEngine;
using Logic.Scripts.GameDomain.MVC.Ui;

namespace Logic.Scripts.GameDomain.Commands {
    public class StartGamePlayStateCommand : BaseCommand, ICommandAsync {
        //Temporario para tirar os asserts

        private IGamePlayUiController _gamePlayUiController;
        private IAudioService _audioService;
        private INaraController _naraController;
        private ICommandFactory _commandFactory;
        private IWorldCameraController _worldCameraController;
        private IGameInputActionsController _gameInputActionsController;
        private IUpdateSubscriptionService _updateSubscriptionService;
        private ICastController _castController;

        private GamePlayInitatorEnterData _enterData;

        public StartGamePlayStateCommand SetEnterData(GamePlayInitatorEnterData enterData) {
            _enterData = enterData;
            return this;
        }

        public override void ResolveDependencies() {
            _audioService = _diContainer.Resolve<IAudioService>();
            //_gamePlayAudioClipsScriptableObject = _diContainer.Resolve<GamePlayAudioClipsScriptableObject>();
            _gamePlayUiController = _diContainer.Resolve<IGamePlayUiController>();
            _naraController = _diContainer.Resolve<INaraController>();
            _commandFactory = _diContainer.Resolve<ICommandFactory>();
            _worldCameraController = _diContainer.Resolve<IWorldCameraController>();
            _gameInputActionsController = _diContainer.Resolve<IGameInputActionsController>();
            _updateSubscriptionService = _diContainer.Resolve<IUpdateSubscriptionService>();
            _castController = _diContainer.Resolve<ICastController>();
        }

        public async Awaitable Execute(CancellationTokenSource cancellationTokenSource) {
            await _gameInputActionsController.WaitForAnyKeyPressed(cancellationTokenSource, true);
            _gameInputActionsController.EnableInputs();
            _gameInputActionsController.RegisterAllInputListeners();
            _naraController.InitEntryPoint();
            _worldCameraController.StartFollowTarget(_naraController.NaraViewGO.transform, _updateSubscriptionService);
            _castController.InitEntryPoint(_naraController);
            await Awaitable.NextFrameAsync();
            _commandFactory.CreateCommandVoid<EnterTurnModeCommand>().Execute();
        }
    }
}