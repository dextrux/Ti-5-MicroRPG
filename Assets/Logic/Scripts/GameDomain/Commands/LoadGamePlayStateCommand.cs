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
    public class LoadGamePlayStateCommand : BaseCommand, ICommandAsync {
        //To-Do depois criar o instanciamento async das fases


        private IGamePlayUiController _gamePlayUiController;
        private IAudioService _audioService;
        private INaraController _naraController;
        //private GamePlayAudioClipsScriptableObject _gamePlayAudioClipsScriptableObject; Lista de audios espec�ficos do gameplay
        private ICommandFactory _commandFactory;
        private IWorldCameraController _worldCameraController;
        private IGameInputActionsController _gameInputActionsController;
        private IUpdateSubscriptionService _updateSubscriptionService;
        private IAbilityController _abilityController;

        private GamePlayInitatorEnterData _enterData;

        public LoadGamePlayStateCommand SetEnterData(GamePlayInitatorEnterData enterData) {
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
            _abilityController = _diContainer.Resolve<IAbilityController>();
        }

        public async Awaitable Execute(CancellationTokenSource cancellationTokenSource) {
            await _gameInputActionsController.WaitForAnyKeyPressed(cancellationTokenSource, true);
            _naraController.InitEntryPoint();
            _gamePlayUiController.TempHoldScreenHide();
            _worldCameraController.StartFollowTarget(_naraController.NaraViewGO.transform, _updateSubscriptionService);
            _gameInputActionsController.EnableInputs();
            _gameInputActionsController.RegisterAllInputListeners();
            _abilityController.InitEntryPoint(_naraController);
            await Awaitable.NextFrameAsync();
            _commandFactory.CreateCommandVoid<EnterTurnModeCommand>().Execute();
            return;
        }
    }
}