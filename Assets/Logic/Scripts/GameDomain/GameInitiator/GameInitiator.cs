using Logic.Scripts.Core.CoreInitiator;
using Logic.Scripts.Core.CoreInitiator.Base;
using Logic.Scripts.Core.Mvc.LoadingScreen;
using Logic.Scripts.GameDomain.States;
using Logic.Scripts.Services.InitiatorInvokerService;
using Logic.Scripts.Services.StateMachineService;
using Logic.Scripts.Utils;
using System.Threading;
using UnityEngine;
namespace Logic.Scripts.GameDomain.GameInitiator {
    public class GameInitiator : ISceneInitiator, IGameInitiator {

        private readonly IStateMachineService _stateMachine;
        private readonly ILoadingScreenController _loadingScreenController;
        private readonly LobbyState.Factory _lobbyStateFactory;
        private readonly ISceneInitiatorsService _sceneInitiatorsService;

        public SceneType SceneType => SceneType.GameScene;

        public GameInitiator(IStateMachineService stateMachine, LobbyState.Factory LobbyStateFactory, ILoadingScreenController loadingScreenController, ISceneInitiatorsService sceneInitiatorsService) {
            _stateMachine = stateMachine;
            _lobbyStateFactory = LobbyStateFactory;
            _loadingScreenController = loadingScreenController;
            _sceneInitiatorsService = sceneInitiatorsService;
            _sceneInitiatorsService.RegisterInitiator(this);
        }


        public async Awaitable LoadEntryPoint(IInitiatorEnterData enterDataObject, CancellationTokenSource cancellationTokenSource) {
            GameInitiatorEnterData enterData = (GameInitiatorEnterData)enterDataObject;
            _ = _loadingScreenController.SetLoadingSlider(0.5f, cancellationTokenSource);
            //await To-do Adicionar informações de load dos níveis
            await _stateMachine.EnterInitialGameState(_lobbyStateFactory.Create(new LobbyInitiatorEnterData()), cancellationTokenSource);
        }

        public Awaitable StartEntryPoint(IInitiatorEnterData enterDataObject, CancellationTokenSource cancellationTokenSource) {
            return AwaitableUtils.CompletedTask;
        }
        public Awaitable InitExitPoint(CancellationTokenSource cancellationTokenSource) {
            _sceneInitiatorsService.UnregisterInitiator(this);
            return AwaitableUtils.CompletedTask;
        }
    }
}