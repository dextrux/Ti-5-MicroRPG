using Logic.Scripts.Services.CommandFactory;
using Logic.Scripts.Turns;

namespace Logic.Scripts.Turns.Actors
{
	public interface ITurnContext
	{
		TurnStateService TurnState { get; }
		IActionPointsService ActionPoints { get; }
		IEchoService Echoes { get; }
		ICommandFactory Commands { get; }
		ITurnActivityTracker Activity { get; }
	}
}


