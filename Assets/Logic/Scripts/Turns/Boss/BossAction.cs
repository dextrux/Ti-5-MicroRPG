using Zenject;

namespace Logic.Scripts.Turns
{
    public class BossActionService : IBossActionService
    {
        private readonly ITurnEventBus _eventBus;

        public BossActionService(ITurnEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public async void ExecuteBossTurn()
        {
            await System.Threading.Tasks.Task.Delay(1000);
            _eventBus.Publish(new BossActionCompletedSignal());
        }
    }
}
