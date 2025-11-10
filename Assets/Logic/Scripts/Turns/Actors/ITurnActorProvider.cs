using System.Collections.Generic;

namespace Logic.Scripts.Turns.Actors
{
	public interface ITurnActorProvider
	{
		// Retorna snapshot ordenado dos atores para a fase informada
		IReadOnlyList<ITurnActor> GetActorsForPhase(TurnPhase phase);
	}
}


