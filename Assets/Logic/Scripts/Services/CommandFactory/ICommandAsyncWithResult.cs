using System.Threading;
using UnityEngine;

namespace Logic.Scripts.Services.CommandFactory
{
    public interface ICommandAsyncWithResult<TReturn> : IBaseCommand
    {
        Awaitable<TReturn> Execute(CancellationTokenSource cancellationTokenSource = null);
    }
}