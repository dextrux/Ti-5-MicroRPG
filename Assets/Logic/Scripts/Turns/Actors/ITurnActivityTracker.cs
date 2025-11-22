using System.Threading;
using System.Threading.Tasks;

namespace Logic.Scripts.Turns.Actors
{
	public interface ITurnActivityTracker
	{
		// Abre uma atividade; use com using para fechar automaticamente no final
		System.IDisposable Begin(string sourceTag);

		// Para diagnóstico/telemetria
		int ActiveCount { get; }

		// Completa quando não há atividades pendentes
		Task WhenIdleAsync(CancellationToken ct = default);
	}
}


