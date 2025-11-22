using Logic.Scripts.Core.Mvc.WorldCamera;
using Logic.Scripts.GameDomain.Commands;
using Logic.Scripts.GameDomain.GameInputActions;
using Logic.Scripts.GameDomain.MVC.Nara;
using Logic.Scripts.GameDomain.MVC.Ui;
using Logic.Scripts.Services.CommandFactory;
using Logic.Scripts.Services.StateMachineService;
using Logic.Scripts.Services.UpdateService;
using Logic.Scripts.Utils;
using System.Threading;
using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Commands.StartLevel {
    public class StartLevelCommand : BaseCommand, ICommandAsync {
        private LoadLevelCommandData _commandData;
        private IGamePlayUiController _gamePlayUiController;
        private INaraController _naraController;
        private IGameInputActionsController _gameInputActionsController;
        private IWorldCameraController _worldCameraController;
        private IStateMachineService _stateMachineService;
        private ILevelCancellationTokenService _levelCancellationTokenService;
        private ILevelsDataService _levelsDataService;
        private IUpdateSubscriptionService _updateSubscriptionService;
        private ICommandFactory _commandFactory;

        public override void ResolveDependencies() {
            _gamePlayUiController = _diContainer.Resolve<IGamePlayUiController>();
            _naraController = _diContainer.Resolve<INaraController>();
            _gameInputActionsController = _diContainer.Resolve<IGameInputActionsController>();
            _worldCameraController = _diContainer.Resolve<IWorldCameraController>();
            _levelCancellationTokenService = _diContainer.Resolve<ILevelCancellationTokenService>();
            _levelsDataService = _diContainer.Resolve<ILevelsDataService>();
            _updateSubscriptionService = _diContainer.Resolve<IUpdateSubscriptionService>();
            _commandFactory = _diContainer.Resolve<ICommandFactory>();
        }

        public async Awaitable Execute(CancellationTokenSource cancellationTokenSource) {
            _gameInputActionsController.RegisterAllInputListeners();
            _worldCameraController.StartFollowTarget(_naraController.NaraViewGO.transform, _updateSubscriptionService);
            _naraController.RegisterListeners();
            await Awaitable.NextFrameAsync();
            //To-Do Unfreeze movement nara
            //Activate GameplayView se necessário
            _commandFactory.CreateCommandVoid<EnterTurnModeCommand>().Execute(); //To-Do verificar se é modo de turno o level
        }
    }
}
