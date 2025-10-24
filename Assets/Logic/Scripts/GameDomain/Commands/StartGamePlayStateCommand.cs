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


        private IGameInputActionsController _gameInputActionsController;

        private GamePlayInitatorEnterData _enterData;

        public StartGamePlayStateCommand SetEnterData(GamePlayInitatorEnterData enterData) {
            _enterData = enterData;
            return this;
        }

        public override void ResolveDependencies() {
            _gameInputActionsController = _diContainer.Resolve<IGameInputActionsController>();
        }

        public async Awaitable Execute(CancellationTokenSource cancellationTokenSource) {
            await _gameInputActionsController.WaitForAnyKeyPressed(cancellationTokenSource, true);
            return;
        }
    }
}