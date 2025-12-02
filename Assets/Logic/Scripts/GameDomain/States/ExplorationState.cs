using Logic.Scripts.Services.AudioService;
using Logic.Scripts.Services.SceneServices;
using Logic.Scripts.Services.StateMachineService;
using System.Threading;
using UnityEngine;
using Zenject;

public class ExplorationState : BaseGameState<ExplorationInitiatorEnterData> {
    private readonly ISceneLoaderService _sceneLoaderService;
    private readonly IAudioService _audio;

    public override GameStateType GameStateType => GameStateType.Exploration;

    public ExplorationState(ISceneLoaderService sceneLoaderService, ExplorationInitiatorEnterData explorationStateEnterData, IAudioService audio) : base(explorationStateEnterData) {
        _sceneLoaderService = sceneLoaderService;
        _audio = audio;
    }

    public override async Awaitable LoadState(CancellationTokenSource cancellationTokenSource) {
        await base.LoadState(cancellationTokenSource);
        await _sceneLoaderService.TryLoadScene(SceneType.ExplorationScene, EnterData, cancellationTokenSource);
        Debug.Log("Load state");
    }

    public override async Awaitable StartState(CancellationTokenSource cancellationTokenSource) {
        await base.StartState(cancellationTokenSource);
        Debug.Log("Start state");
        await _sceneLoaderService.StartScene(SceneType.ExplorationScene, EnterData, cancellationTokenSource);
    }

    public override async Awaitable ExitState(CancellationTokenSource cancellationTokenSource) {
        await base.ExitState(cancellationTokenSource);
        await _sceneLoaderService.TryUnloadScene(SceneType.ExplorationScene, cancellationTokenSource);
    }

    public class Factory : PlaceholderFactory<ExplorationInitiatorEnterData, ExplorationState> { }
}
