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
        private ICastController _castController;
        private ICustomizeUIController _customizeUIController;

        public override void ResolveDependencies() {
            _gamePlayUiController = _diContainer.Resolve<IGamePlayUiController>();
            _naraController = _diContainer.Resolve<INaraController>();
            _gameInputActionsController = _diContainer.Resolve<IGameInputActionsController>();
            _worldCameraController = _diContainer.Resolve<IWorldCameraController>();
            _levelsDataService = _diContainer.Resolve<ILevelsDataService>();
            _gamePlayDataService = _diContainer.Resolve<IGamePlayDataService>();
            _updateSubscriptionService = _diContainer.Resolve<IUpdateSubscriptionService>();
            _commandFactory = _diContainer.Resolve<ICommandFactory>();
            _customizeUIController = _diContainer.Resolve<ICustomizeUIController>();
            if (_levelsDataService.GetLevelData(_gamePlayDataService.CurrentLevelNumber).ControllerType == typeof(NaraTurnMovementController)) {
                _castController = _diContainer.Resolve<ICastController>();
                _bossController = _diContainer.Resolve<IBossController>();
            }
        }

        public async Awaitable Execute(CancellationTokenSource cancellationTokenSource) {
            _gameInputActionsController.RegisterAllInputListeners();
            _worldCameraController.StartFollowTarget(_naraController.NaraViewGO.transform, _updateSubscriptionService);
            _naraController.RegisterListeners();
            _naraController.InitEntryPoint();
			// Set initial player position using fixed world position from LevelTurnData -> BossConfiguration
			try {
				var naraGO = _naraController.NaraViewGO;
				if (naraGO != null) {
					Vector3 desired = new Vector3(0f, 0f, -10f);
					var levelData = _levelsDataService.GetLevelData(_gamePlayDataService.CurrentLevelNumber);
					if (levelData is LevelTurnData ltd && ltd.BossConfiguration != null) {
						desired = ltd.BossConfiguration.InitialPlayerPosition;
					}
					_naraController.SetPosition(desired);
					// Align movement center to the new spawn so clamps don't drag Nara elsewhere before PlayerAct
					if (levelData is LevelTurnData) {
						if (_naraController.NaraMove is NaraTurnMovementController tm) {
							tm.SetMovementRadiusCenter();
						}
					}
				}
			} catch { }
            _customizeUIController.InitEntryPoint();
            await Awaitable.NextFrameAsync();
            //To-Do Unfreeze movement nara
            //Activate GameplayView se necess�rio
            if (_levelsDataService.GetLevelData(_gamePlayDataService.CurrentLevelNumber).ControllerType == typeof(NaraTurnMovementController)) {
                _castController.InitEntryPoint(_naraController);
                _bossController.Initialize();
                _commandFactory.CreateCommandVoid<EnterTurnModeCommand>().Execute();
            }

        }
    }
}
