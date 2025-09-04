using Logic.Scripts.Core.CoreInitiator.Base;
using UnityEngine;

namespace Logic.Scripts.Services.StateMachineService
{
    public interface IStateInitiator<T> where T : class, IInitiatorEnterData
    {
        Awaitable EnterState(T stateEnterData = null);
        Awaitable ExitState();
    }
}