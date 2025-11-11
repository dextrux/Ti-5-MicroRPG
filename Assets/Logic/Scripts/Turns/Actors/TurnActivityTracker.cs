using System.Threading;
using System.Threading.Tasks;

namespace Logic.Scripts.Turns.Actors
{
	public sealed class TurnActivityTracker : ITurnActivityTracker
	{
		private int _active;
		private TaskCompletionSource<bool> _tcs =
			new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

		public int ActiveCount => System.Threading.Interlocked.CompareExchange(ref _active, 0, 0);

		public System.IDisposable Begin(string sourceTag)
		{
			System.Threading.Interlocked.Increment(ref _active);
			return new Handle(this);
		}

		public Task WhenIdleAsync(CancellationToken ct = default)
		{
			if (ActiveCount == 0) return Task.CompletedTask;
			if (ct.CanBeCanceled)
			{
				ct.Register(() => _tcs.TrySetCanceled(ct));
			}
			return _tcs.Task;
		}

		private void CompleteIfIdle()
		{
			if (ActiveCount == 0)
			{
				var old = _tcs;
				_tcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
				old.TrySetResult(true);
			}
		}

		private sealed class Handle : System.IDisposable
		{
			private TurnActivityTracker _owner;
			public Handle(TurnActivityTracker owner) { _owner = owner; }
			public void Dispose()
			{
				if (_owner == null) return;
				var n = System.Threading.Interlocked.Decrement(ref _owner._active);
				if (n <= 0) _owner.CompleteIfIdle();
				_owner = null;
			}
		}
	}
}


