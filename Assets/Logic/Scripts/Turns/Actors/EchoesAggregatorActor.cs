using System.Threading;
using System.Threading.Tasks;

namespace Logic.Scripts.Turns.Actors
{
	// Placeholder agregador: resolve ecos pendentes via IEchoService
	// Quando existirem Echo units com Task própria, crie 1 actor por unidade.
	public sealed class EchoesAggregatorActor : TurnActorBase
	{
		private readonly IEchoService _echoes;

		public EchoesAggregatorActor(IEchoService echoes)
			: base("Echoes", TurnPhase.EchoesAct)
		{
			_echoes = echoes;
		}

		protected override async Task OnExecuteTurnAsync(ITurnContext ctx, CancellationToken ct)
		{
			if (_echoes != null)
			{
				await _echoes.ResolveDueEchoesAsync().ConfigureAwait(false);
			}
		}
	}
}


