using Logic.Scripts.Services.SceneServices;
using Logic.Scripts.Services.StateMachineService;
using System.Threading;
using UnityEngine;
using Zenject;
using Logic.Scripts.Services.AudioService;

public class GamePlayState : BaseGameState<GamePlayInitatorEnterData> {
    private readonly ISceneLoaderService _sceneLoaderService;
    private readonly IAudioService _audio;

    public override GameStateType GameStateType => GameStateType.GamePlay;

    public GamePlayState(ISceneLoaderService sceneLoaderService, GamePlayInitatorEnterData gamePlayStateEnterData, IAudioService audio) : base(gamePlayStateEnterData) {
        _sceneLoaderService = sceneLoaderService;
        _audio = audio;
    }

    public override async Awaitable LoadState(CancellationTokenSource cancellationTokenSource) {
        await base.LoadState(cancellationTokenSource);
        await _sceneLoaderService.TryLoadScene(SceneType.GamePlayScene, EnterData, cancellationTokenSource);
    }

    public override async Awaitable StartState(CancellationTokenSource cancellationTokenSource) {
        await base.StartState(cancellationTokenSource);
        _audio.PlayAudio(global::AudioClipType.BossTheme, AudioChannelType.Music, AudioPlayType.Loop);
        await _sceneLoaderService.StartScene(SceneType.GamePlayScene, EnterData, cancellationTokenSource);
    }

    public override async Awaitable ExitState(CancellationTokenSource cancellationTokenSource) {
        await base.ExitState(cancellationTokenSource);
        await _sceneLoaderService.TryUnloadScene(SceneType.GamePlayScene, cancellationTokenSource);
    }

    public class Factory : PlaceholderFactory<GamePlayInitatorEnterData, GamePlayState> { }
}
