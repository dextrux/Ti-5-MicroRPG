using System.Threading.Tasks;

namespace Logic.Scripts.Turns
{
    public interface IEnviromentActionService
    {
        void ExecuteEnviromentTurn();
        Task ExecuteEnviromentTurnAsync();
    }
}
