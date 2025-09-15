using System.Threading;
using UnityEngine;

namespace Logic.Scripts.Services.StateMachineService
{
    public interface IGameState
    {
        CancellationTokenSource CancellationTokenSource { get; }
        GameStateType GameStateType { get; }
        Awaitable LoadState(CancellationTokenSource cancellationTokenSource);
        Awaitable StartState(CancellationTokenSource cancellationTokenSource);
        Awaitable ExitState(CancellationTokenSource cancellationTokenSource);
    }
}