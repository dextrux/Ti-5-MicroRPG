
using System.Threading;
using UnityEngine;

namespace Logic.Scripts.Services.StateMachineService {
    public interface IStateMachineService {
        IGameState CurrentState();
        Awaitable EnterInitialGameState(IGameState initialState, CancellationTokenSource cancellationTokenSource);
        void SwitchState(IGameState newState);
    }
}