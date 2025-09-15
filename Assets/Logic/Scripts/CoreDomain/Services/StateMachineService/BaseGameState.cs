using System.Threading;
using Logic.Scripts.Core.CoreInitiator.Base;
using Logic.Scripts.Services.Logger.Base;
using Logic.Scripts;
using UnityEngine;
using Logic.Scripts.Utils;

namespace Logic.Scripts.Services.StateMachineService
{
    public abstract class BaseGameState<T> : IGameState where T : class, IInitiatorEnterData
    {
        private readonly CancellationTokenSource _cancellationTokenSource;
        public T EnterData { get; }

        protected BaseGameState(T enterData)
        {
            EnterData = enterData;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public CancellationTokenSource CancellationTokenSource => CancellationTokenSource.CreateLinkedTokenSource(_cancellationTokenSource.Token);
        public abstract GameStateType GameStateType { get; }

        public virtual Awaitable LoadState(CancellationTokenSource cancellationTokenSource)
        {
            LogService.LogTopic($"Load state {GameStateType}", LogTopicType.GameState);
            return AwaitableUtils.CompletedTask;
        }
        
        public virtual Awaitable StartState(CancellationTokenSource cancellationTokenSource)
        {
            LogService.LogTopic($"Start state {GameStateType}", LogTopicType.GameState);
            return AwaitableUtils.CompletedTask;
        }

        public virtual Awaitable ExitState(CancellationTokenSource cancellationTokenSource)
        {
            _cancellationTokenSource.Cancel();
            return AwaitableUtils.CompletedTask;
        }
    }
}