using Logic.Scripts.Core.CoreInitiator.Base;
using System.Threading;
using UnityEngine;

namespace Logic.Scripts.Services.InitiatorInvokerService {
    public interface ISceneInitiatorsService {
        void RegisterInitiator(ISceneInitiator sceneInitiator);
        void UnregisterInitiator(ISceneInitiator sceneInitiator);
        Awaitable InvokeInitiatorLoadEntryPoint(SceneType sceneType, IInitiatorEnterData enterData, CancellationTokenSource cancellationTokenSource);
        Awaitable InvokeInitiatorStartEntryPoint(SceneType sceneType, IInitiatorEnterData enterData, CancellationTokenSource cancellationTokenSource);
        Awaitable InvokeInitiatorExitPoint(SceneType sceneType, CancellationTokenSource cancellationTokenSource);
    }
}