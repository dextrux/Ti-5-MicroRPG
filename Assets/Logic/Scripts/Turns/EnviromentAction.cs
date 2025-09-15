using Zenject;

namespace Logic.Scripts.Turns
{
    public class EnviromentActionService : IEnviromentActionService
    {
        private readonly ITurnEventBus _eventBus;

        public EnviromentActionService(ITurnEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public async void ExecuteEnviromentTurn()
        {
            await System.Threading.Tasks.Task.Delay(500);
            _eventBus.Publish(new EnviromentActionCompletedSignal());
        }
    }
}
