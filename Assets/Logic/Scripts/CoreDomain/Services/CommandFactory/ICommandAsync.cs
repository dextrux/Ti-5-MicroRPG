using System.Threading;
using UnityEngine;

namespace Logic.Scripts.Services.CommandFactory
{
    public interface ICommandAsync : IBaseCommand
    {
        Awaitable Execute(CancellationTokenSource cancellationTokenSource);
    }
}