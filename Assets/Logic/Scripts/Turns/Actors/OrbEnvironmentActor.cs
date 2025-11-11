using System.Threading;
using System.Threading.Tasks;
using Logic.Scripts.GameDomain.MVC.Environment.Orb;

namespace Logic.Scripts.Turns.Actors
{
	public sealed class OrbEnvironmentActor : TurnActorBase
	{
		private readonly OrbController _orb;

		public OrbEnvironmentActor(OrbController orb)
			: base(orb != null ? orb.name : "Orb", TurnPhase.EnviromentAct)
		{
			_orb = orb;
		}

		public override bool IsAlive => _orb != null && _orb.gameObject != null;

		protected override async Task OnExecuteTurnAsync(ITurnContext ctx, CancellationToken ct)
		{
			if (_orb == null) return;
			using (ctx.Activity.Begin("Environment/OrbTick"))
			{
				_orb.StartTickAsync();
				var t = _orb.CurrentTickTask ?? Task.CompletedTask;
				await t.ConfigureAwait(false);
			}
		}
	}
}


