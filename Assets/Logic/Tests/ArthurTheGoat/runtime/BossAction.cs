using Zenject;

namespace Logic.Tests.ArthurTheGoat.Turns
{
    public class BossActionService : IBossActionService
    {
        readonly ITurnEventBus _eventBus;

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


