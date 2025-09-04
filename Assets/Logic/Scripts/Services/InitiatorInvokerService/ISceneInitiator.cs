using Logic.Scripts.Core.CoreInitiator.Base;
using System.Threading;
using UnityEngine;

namespace Logic.Scripts.Services.InitiatorInvokerService {
    public interface ISceneInitiator {
        SceneType SceneType { get; }
        Awaitable LoadEntryPoint(IInitiatorEnterData enterDataObject, CancellationTokenSource cancellationTokenSource);
        Awaitable StartEntryPoint(IInitiatorEnterData enterDataObject, CancellationTokenSource cancellationTokenSource);
        Awaitable InitExitPoint(CancellationTokenSource cancellationTokenSource);
    }
}