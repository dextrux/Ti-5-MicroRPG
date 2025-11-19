using System.Threading.Tasks;

namespace Logic.Scripts.Turns
{
	public interface IEnvironmentTurnActor
	{
		Task ExecuteAsync();
		bool RemoveAfterRun { get; }
	}
}
