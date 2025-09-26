using Zenject;

namespace Logic.Scripts.Turns
{
    public class EnviromentActionService : IEnviromentActionService
    {
        private readonly System.Collections.Generic.IEnumerable<IEnviromentRule> _rules;

        public EnviromentActionService(System.Collections.Generic.IEnumerable<IEnviromentRule> rules)
        {
            _rules = rules;
        }

        public async void ExecuteEnviromentTurn()
        {
            await ExecuteEnviromentTurnAsync();
        }

        public async System.Threading.Tasks.Task ExecuteEnviromentTurnAsync()
        {
            foreach (IEnviromentRule rule in _rules)
            {
                rule.Execute();
            }
            await System.Threading.Tasks.Task.Delay(500);
        }
    }
}
