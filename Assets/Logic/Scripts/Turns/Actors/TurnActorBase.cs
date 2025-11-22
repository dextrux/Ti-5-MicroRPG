using System.Threading;
using System.Threading.Tasks;

namespace Logic.Scripts.Turns.Actors
{
	public abstract class TurnActorBase : ITurnActor
	{
		public string Name { get; }
		public TurnPhase Phase { get; }
		public virtual bool IsAlive => true;

		protected TurnActorBase(string name, TurnPhase phase)
		{
			Name = name;
			Phase = phase;
		}

		public async Task ExecuteTurnAsync(ITurnContext context, CancellationToken ct)
		{
			await OnExecuteTurnAsync(context, ct).ConfigureAwait(false);
			await context.Activity.WhenIdleAsync(ct).ConfigureAwait(false);
		}

		protected abstract Task OnExecuteTurnAsync(ITurnContext context, CancellationToken ct);
	}
}


