using Zenject;

namespace Logic.Scripts.Turns
{
    public class EnviromentActionService : IEnviromentActionService
    {
        private readonly ITurnEventBus _eventBus;
        private readonly System.Collections.Generic.IEnumerable<IEnviromentRule> _rules;

        public EnviromentActionService(ITurnEventBus eventBus, System.Collections.Generic.IEnumerable<IEnviromentRule> rules)
        {
            _eventBus = eventBus;
            _rules = rules;
        }

        public async void ExecuteEnviromentTurn()
        {
            foreach (IEnviromentRule rule in _rules)
            {
                rule.Execute();
            }
            await System.Threading.Tasks.Task.Delay(500);
            _eventBus.Publish(new EnviromentActionCompletedSignal());
        }
    }
}
