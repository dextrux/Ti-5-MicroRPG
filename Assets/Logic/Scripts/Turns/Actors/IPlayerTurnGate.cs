using System.Threading;
using System.Threading.Tasks;

namespace Logic.Scripts.Turns.Actors
{
	public interface IPlayerTurnGate
	{
		// Chame ao clicar em "Next Turn"
		void SignalPlayerEndedTurn();
		// Aguardará até o jogador sinalizar
		Task WaitForPlayerEndAsync(CancellationToken ct);
	}
}


