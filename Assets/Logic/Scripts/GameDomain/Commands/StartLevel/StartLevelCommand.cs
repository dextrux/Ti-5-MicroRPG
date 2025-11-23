using Logic.Scripts.Core.Mvc.WorldCamera;
using Logic.Scripts.GameDomain.Commands;
using Logic.Scripts.GameDomain.GameInputActions;
using Logic.Scripts.GameDomain.MVC.Boss;
using Logic.Scripts.GameDomain.MVC.Nara;
using Logic.Scripts.GameDomain.MVC.Ui;
using Logic.Scripts.Services.CommandFactory;
using Logic.Scripts.Services.UpdateService;
using System.Threading;
using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Commands.StartLevel {
    public class StartLevelCommand : BaseCommand, ICommandAsync {

        private IGamePlayUiController _gamePlayUiController;
        private INaraController _naraController;
        private IGameInputActionsController _gameInputActionsController;
        private IWorldCameraController _worldCameraController;
        private ILevelsDataService _levelsDataService;
        private IUpdateSubscriptionService _updateSubscriptionService;
        private ICommandFactory _commandFactory;
        private IGamePlayDataService _gamePlayDataService;
        private IBossController _bossController;

        public override void ResolveDependencies() {
            _gamePlayUiController = _diContainer.Resolve<IGamePlayUiController>();
            _naraController = _diContainer.Resolve<INaraController>();
            _gameInputActionsController = _diContainer.Resolve<IGameInputActionsController>();
            _worldCameraController = _diContainer.Resolve<IWorldCameraController>();
            _levelsDataService = _diContainer.Resolve<ILevelsDataService>();
            _gamePlayDataService = _diContainer.Resolve<IGamePlayDataService>();
            _updateSubscriptionService = _diContainer.Resolve<IUpdateSubscriptionService>();
            _commandFactory = _diContainer.Resolve<ICommandFactory>();
            if (_levelsDataService.GetLevelData(_gamePlayDataService.CurrentLevelNumber).ControllerType == typeof(NaraTurnMovementController)) {
                _bossController = _diContainer.Resolve<IBossController>();
            }
        }

        public async Awaitable Execute(CancellationTokenSource cancellationTokenSource) {
            _gameInputActionsController.RegisterAllInputListeners();
            _worldCameraController.StartFollowTarget(_naraController.NaraViewGO.transform, _updateSubscriptionService);
            _naraController.RegisterListeners();
            _naraController.InitEntryPoint();
            await Awaitable.NextFrameAsync();
            //To-Do Unfreeze movement nara
            //Activate GameplayView se necessário
            if (_levelsDataService.GetLevelData(_gamePlayDataService.CurrentLevelNumber).ControllerType == typeof(NaraTurnMovementController)) {
                _bossController.Initialize();
                _commandFactory.CreateCommandVoid<EnterTurnModeCommand>().Execute();
            }

        }
    }
}
