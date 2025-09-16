using System.Threading;
using UnityEngine;

namespace Logic.Scripts.GameDomain.GameInputActions {
    public interface IGameInputActionsController {
        void EnableInputs();
        void DisableInputs();
        void RegisterAllInputListeners();
        void UnregisterAllInputListeners();
        Awaitable WaitForAnyKeyPressed(CancellationTokenSource cancellationTokenSource, bool canPressOverGui);
    }
}
