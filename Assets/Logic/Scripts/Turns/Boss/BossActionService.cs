using Zenject;
using Logic.Scripts.GameDomain.MVC.Boss;
using System.Threading.Tasks;

namespace Logic.Scripts.Turns
{
    public class BossActionService : IBossActionService
    {
        private readonly IBossController _bossController;

        public BossActionService(IBossController bossController)
        {
            _bossController = bossController;
        }

        public async void ExecuteBossTurn()
        {
            await ExecuteBossTurnAsync();
        }

        public async Task ExecuteBossTurnAsync()
        {
            _bossController.PlanNextTurn();
            _bossController.ExecuteTurn();
            await System.Threading.Tasks.Task.Yield();
        }
    }
}


