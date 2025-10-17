using Logic.Scripts.Core.CoreInitiator.Base;
using Logic.Scripts.Services.CommandFactory;
using Logic.Scripts.Services.InitiatorInvokerService;
using System.Threading;
using UnityEngine;

public class LobbyInitiator : ISceneInitiator, ILobbyInitiator {
    private readonly ICommandFactory _commandFactory;
    private readonly ISceneInitiatorsService _sceneInitiatorsService;

    public SceneType SceneType => SceneType.LobbyScene;

    public Awaitable InitExitPoint(CancellationTokenSource cancellationTokenSource) {
        throw new System.NotImplementedException();
    }

    public Awaitable LoadEntryPoint(IInitiatorEnterData enterDataObject, CancellationTokenSource cancellationTokenSource) {
        throw new System.NotImplementedException();
    }

    public Awaitable StartEntryPoint(IInitiatorEnterData enterDataObject, CancellationTokenSource cancellationTokenSource) {
        throw new System.NotImplementedException();
    }
}
