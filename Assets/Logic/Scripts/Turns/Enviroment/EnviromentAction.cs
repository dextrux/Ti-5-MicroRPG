using Zenject;

namespace Logic.Scripts.Turns
{
    public class EnviromentActionService : IEnviromentActionService
    {
        private readonly System.Collections.Generic.IEnumerable<IEnviromentRule> _rules;
        private readonly System.Collections.Generic.IEnumerable<IEnviromentAsyncRule> _asyncRules;

        public EnviromentActionService(System.Collections.Generic.IEnumerable<IEnviromentRule> rules,
            System.Collections.Generic.IEnumerable<IEnviromentAsyncRule> asyncRules)
        {
            _rules = rules;
            _asyncRules = asyncRules;
        }

        public async void ExecuteEnviromentTurn()
        {
            await ExecuteEnviromentTurnAsync();
        }

        public async System.Threading.Tasks.Task ExecuteEnviromentTurnAsync()
        {
            UnityEngine.Debug.Log("[Environment] Begin ExecuteEnviromentTurnAsync");
            foreach (IEnviromentRule rule in _rules)
            {
                UnityEngine.Debug.Log($"[Environment] Execute rule: {rule.GetType().Name}");
                rule.Execute();
            }
            // Await async rules if any are bound
            int asyncCount = 0;
            if (_asyncRules != null)
            {
                foreach (var r in _asyncRules) asyncCount++;
                UnityEngine.Debug.Log($"[Environment] Awaiting async rules (injected): {asyncCount}");
                foreach (var r in _asyncRules)
                {
                    UnityEngine.Debug.Log($"[Environment] ExecuteAsync rule: {r.GetType().Name}");
                    await r.ExecuteAsync();
                }
            }
            else
            {
                var asyncRulesFallback = Zenject.ProjectContext.Instance.Container.ResolveAll<IEnviromentAsyncRule>();
                UnityEngine.Debug.Log($"[Environment] Awaiting async rules (fallback): {asyncRulesFallback.Count}");
                for (int i = 0; i < asyncRulesFallback.Count; i++)
                {
                    UnityEngine.Debug.Log($"[Environment] ExecuteAsync rule: {asyncRulesFallback[i].GetType().Name}");
                    await asyncRulesFallback[i].ExecuteAsync();
                }
            }
            UnityEngine.Debug.Log("[Environment] End ExecuteEnviromentTurnAsync");
        }
    }
}
