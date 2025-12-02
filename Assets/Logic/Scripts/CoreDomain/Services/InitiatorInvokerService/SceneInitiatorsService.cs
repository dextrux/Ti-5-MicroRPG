using System.Collections.Generic;
using System.Threading;
using Logic.Scripts.Core.CoreInitiator.Base;
using Logic.Scripts.Services.SceneServices;
using UnityEngine;

namespace Logic.Scripts.Services.InitiatorInvokerService {
    public class SceneInitiatorsService : ISceneInitiatorsService {
        private readonly Dictionary<SceneType, ISceneInitiator> _sceneInitiators = new Dictionary<SceneType, ISceneInitiator>();

        public void RegisterInitiator(ISceneInitiator sceneInitiator) {
            _sceneInitiators.Add(sceneInitiator.SceneType, sceneInitiator);
        }

        public void UnregisterInitiator(ISceneInitiator sceneInitiator) {
            _sceneInitiators.Remove(sceneInitiator.SceneType);
        }

        public async Awaitable InvokeInitiatorLoadEntryPoint(SceneType sceneType, IInitiatorEnterData enterData, CancellationTokenSource cancellationTokenSource) {
            Debug.Log("Chegou no dict");
            Debug.Log("Is scene on Dictionary: " + (_sceneInitiators.ContainsKey(sceneType)));
            await _sceneInitiators[sceneType].LoadEntryPoint(enterData, cancellationTokenSource);
        }

        public async Awaitable InvokeInitiatorStartEntryPoint(SceneType sceneType, IInitiatorEnterData enterData, CancellationTokenSource cancellationTokenSource) {
            await _sceneInitiators[sceneType].StartEntryPoint(enterData, cancellationTokenSource);
        }

        public async Awaitable InvokeInitiatorExitPoint(SceneType sceneType, CancellationTokenSource cancellationTokenSource) {
            await _sceneInitiators[sceneType].InitExitPoint(cancellationTokenSource);
        }
    }
}
