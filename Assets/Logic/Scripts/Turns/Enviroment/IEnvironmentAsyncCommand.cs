using System.Threading.Tasks;

namespace Logic.Scripts.Turns
{
	public interface IEnvironmentAsyncCommand
	{
		Task ExecuteAsync();
	}
}
