using System.Threading;
using System.Threading.Tasks;
using Logic.Scripts.GameDomain.MVC.Boss;

namespace Logic.Scripts.Turns.Actors
{
	public sealed class BossActor : TurnActorBase
	{
		private readonly IBossController _boss;

		public override bool IsAlive => _boss != null;

		public BossActor(IBossController boss)
			: base("Boss", TurnPhase.BossAct)
		{
			_boss = boss;
		}

		protected override async Task OnExecuteTurnAsync(ITurnContext ctx, CancellationToken ct)
		{
			if (_boss == null) return;
			_boss.PlanNextTurn();
			await _boss.ExecuteTurnAsync().ConfigureAwait(false);
			// Se o boss disparar efeitos assíncronos fora desta Task,
			// eles devem usar ctx.Activity.Begin/Dispose para que o base aguarde.
		}
	}
}


