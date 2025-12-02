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
using Logic.Scripts.Services.Logger.Base;

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
			// 1) Position player BEFORE listeners/camera to avoid any clamp or unwanted movement overriding spawn
			try {
				Vector3 desired = new Vector3(0f, 0f, -10f);
				var levelData = _levelsDataService.GetLevelData(_gamePlayDataService.CurrentLevelNumber);
				if (levelData is LevelTurnData ltd && ltd.BossConfiguration != null) {
					LogService.Log($"[StartLevel] Using LevelTurnData.BossConfiguration.InitialPlayerPosition as spawn.");
					desired = ltd.BossConfiguration.InitialPlayerPosition;
				} else {
					LogService.Log($"[StartLevel] Using fallback spawn (0,0,-10). LevelData is not LevelTurnData or BossConfiguration is null.");
				}
				LogService.Log($"[StartLevel] Desired player spawn for level {_gamePlayDataService.CurrentLevelNumber}: {desired}");
				_naraController.SetPosition(desired); // sets Rigidbody.position
				// Force immediate transform sync and clear motion so we can read exact spawn this frame
				if (_naraController.NaraViewGO != null) {
					_naraController.NaraViewGO.transform.position = desired;
					var rb = _naraController.NaraViewGO.GetComponent<Rigidbody>();
					if (rb != null) {
						rb.linearVelocity = Vector3.zero;
						rb.angularVelocity = Vector3.zero;
					}
				}
				var actualAfterSet = _naraController.NaraViewGO.transform.position;
				LogService.Log($"[StartLevel] Player position after SetPosition: {actualAfterSet}");
			} catch { }

			// 2) Initialize Nara movement/camera, then recenter and restore radius to avoid initial clamp
			_naraController.InitEntryPoint();
			if (_naraController.NaraMove is NaraTurnMovementController tmInit) {
				tmInit.SetMovementRadiusCenter();
				tmInit.ResetMovementRadius();
				LogService.Log($"[StartLevel] Movement center set to: {tmInit.GetNaraCenter()} | radius: {tmInit.GetNaraRadius()}");
			}

			// 3) Only after spawn is set and movement initialized, enable inputs, camera follow and listeners
			_gameInputActionsController.RegisterAllInputListeners();
			_worldCameraController.StartFollowTarget(_naraController.NaraViewGO.transform, _updateSubscriptionService);
			_naraController.RegisterListeners();
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
