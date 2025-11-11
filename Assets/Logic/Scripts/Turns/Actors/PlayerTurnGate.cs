using System.Threading;
using System.Threading.Tasks;

namespace Logic.Scripts.Turns.Actors
{
	public sealed class PlayerTurnGate : IPlayerTurnGate
	{
		private TaskCompletionSource<bool> _tcs =
			new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

		public void SignalPlayerEndedTurn()
		{
			var old = _tcs;
			_tcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
			old.TrySetResult(true);
		}

		public Task WaitForPlayerEndAsync(CancellationToken ct)
		{
			if (ct.CanBeCanceled)
			{
				ct.Register(() => _tcs.TrySetCanceled(ct));
			}
			return _tcs.Task;
		}
	}
}


