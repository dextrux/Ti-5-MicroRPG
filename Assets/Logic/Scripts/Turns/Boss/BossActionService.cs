using Zenject;
using Logic.Scripts.GameDomain.MVC.Boss;

namespace Logic.Scripts.Turns
{
    public class BossActionService : IBossActionService
    {
        private readonly ITurnEventBus _eventBus;
        private readonly IBossController _bossController;

        public BossActionService(ITurnEventBus eventBus, IBossController bossController)
        {
            _eventBus = eventBus;
            _bossController = bossController;
        }

        public async void ExecuteBossTurn()
        {
            _bossController.PlanNextTurn();
            _bossController.ExecuteTurn();
            await System.Threading.Tasks.Task.Yield();
            _eventBus.Publish(new BossActionCompletedSignal());
        }
    }
}


