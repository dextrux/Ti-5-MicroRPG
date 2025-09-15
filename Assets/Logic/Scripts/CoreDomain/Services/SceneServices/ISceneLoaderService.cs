using Logic.Scripts.Core.CoreInitiator.Base;
using System.Threading;
using UnityEngine;

namespace Logic.Scripts.Services.SceneServices {
    public interface ISceneLoaderService {
        void InitEntryPoint();
        Awaitable<bool> TryLoadScene<TEnterData>(SceneType sceneType, TEnterData enterData, CancellationTokenSource cancellationTokenSource) where TEnterData : class, IInitiatorEnterData;
        Awaitable StartScene<TEnterData>(SceneType gamePlayScene, TEnterData enterData, CancellationTokenSource cancellationTokenSource) where TEnterData : class, IInitiatorEnterData;
        Awaitable<bool> TryUnloadScene(SceneType sceneType, CancellationTokenSource cancellationTokenSource);
    }
}