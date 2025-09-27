using Zenject;
using Logic.Scripts.GameDomain.MVC.Boss;
using System.Threading.Tasks;

namespace Logic.Scripts.Turns
{
    public class BossActionService : IBossActionService
    {
        private readonly IBossController _bossController;
        private bool _isFirstBossTurn = true;

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
            if (_isFirstBossTurn)
            {
                await Task.Delay(3000);
                _isFirstBossTurn = false;
            }
            else
            {
                await Task.Delay(1500);
            }

            _bossController.PlanNextTurn();
            await _bossController.ExecuteTurnAsync();
            await Task.Yield();
        }
    }
}


