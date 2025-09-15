using Logic.Scripts.Services.SceneServices;
using Logic.Scripts.Services.StateMachineService;
using System.Threading;
using UnityEngine;
using Zenject;

namespace Logic.Scripts.GameDomain.States {
    public class LobbyState : BaseGameState<LobbyInitiatorEnterData> {
        private readonly ISceneLoaderService _sceneLoaderService;

        public override GameStateType GameStateType => GameStateType.Lobby;

        public LobbyState(ISceneLoaderService sceneLoaderService, LobbyInitiatorEnterData gamePlayStateEnterData) : base(gamePlayStateEnterData) {
            _sceneLoaderService = sceneLoaderService;
        }

        public override async Awaitable LoadState(CancellationTokenSource cancellationTokenSource) {
            await base.LoadState(cancellationTokenSource);
            await _sceneLoaderService.TryLoadScene(SceneType.LobbyScene, new LobbyInitiatorEnterData(), cancellationTokenSource);
        }

        public override async Awaitable ExitState(CancellationTokenSource cancellationTokenSource) {
            await base.ExitState(cancellationTokenSource);
            await _sceneLoaderService.TryUnloadScene(SceneType.LobbyScene, cancellationTokenSource);
        }

        public class Factory : PlaceholderFactory<LobbyInitiatorEnterData, LobbyState> {
        }
    }
}
