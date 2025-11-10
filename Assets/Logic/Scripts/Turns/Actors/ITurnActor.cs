using System.Threading;
using System.Threading.Tasks;

namespace Logic.Scripts.Turns.Actors
{
	public interface ITurnActor
	{
		string Name { get; }
		TurnPhase Phase { get; }
		bool IsAlive { get; }

		// Deve aguardar todas as ações/animações terminarem antes de concluir
		Task ExecuteTurnAsync(ITurnContext context, CancellationToken ct);
	}
}


