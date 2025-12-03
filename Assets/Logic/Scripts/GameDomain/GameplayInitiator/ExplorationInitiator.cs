using Logic.Scripts.Core.CoreInitiator.Base;
using Logic.Scripts.Services.CommandFactory;
using Logic.Scripts.Services.InitiatorInvokerService;
using Logic.Scripts.Utils;
using System.Threading;
using UnityEngine;

public class ExplorationInitiator : ISceneInitiator, IExplorationInitiator {
    private readonly ICommandFactory _commandFactory;
    private readonly ISceneInitiatorsService _sceneInitiatorsService;

    public SceneType SceneType => SceneType.ExplorationScene;

    public ExplorationInitiator(ICommandFactory commandFactory, ISceneInitiatorsService sceneInitiatorsService) {
        _commandFactory = commandFactory;
        _sceneInitiatorsService = sceneInitiatorsService;
        _sceneInitiatorsService.RegisterInitiator(this);
    }

    public async Awaitable LoadEntryPoint(IInitiatorEnterData enterDataObject, CancellationTokenSource cancellationTokenSource) {
        var enterData = (ExplorationInitiatorEnterData)enterDataObject;
        await _commandFactory.CreateCommandAsync<LoadExplorationStateCommand>().SetEnterData(enterData).Execute(cancellationTokenSource);
    }

    public async Awaitable StartEntryPoint(IInitiatorEnterData enterDataObject, CancellationTokenSource cancellationTokenSource) {
        var enterData = (ExplorationInitiatorEnterData)enterDataObject;
        await _commandFactory.CreateCommandAsync<StartExplorationStateCommand>().SetEnterData(enterData).Execute(cancellationTokenSource);
    }

    public Awaitable InitExitPoint(CancellationTokenSource cancellationTokenSource) {
        _sceneInitiatorsService.UnregisterInitiator(this);
        _commandFactory.CreateCommandVoid<ExitExplorationStateCommand>().Execute();
        return AwaitableUtils.CompletedTask;
    }
}
