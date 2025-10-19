using Logic.Scripts.GameDomain.MVC.Environment.Orb;

namespace Logic.Scripts.Turns
{
    public class OrbEnvironmentRule : IEnviromentRule, IEnviromentAsyncRule
    {
        public OrbEnvironmentRule() {}
        public void Execute()
        {
            var alt = OrbController.Instances;
            UnityEngine.Debug.Log($"[Environment][OrbRule] Orbs: {alt.Count}");
            for (int i = 0; i < alt.Count; i++)
            {
                if (alt[i] != null)
                {
                    UnityEngine.Debug.Log($"[Environment][OrbRule] StartTick -> {alt[i].name}");
                    alt[i].StartTickAsync();
                }
            }
        }

        public async System.Threading.Tasks.Task ExecuteAsync()
        {
            var alt = OrbController.Instances;
            var tasks = new System.Collections.Generic.List<System.Threading.Tasks.Task>(alt.Count);
            for (int i = 0; i < alt.Count; i++)
            {
                if (alt[i] != null && alt[i].CurrentTickTask != null) tasks.Add(alt[i].CurrentTickTask);
            }
            if (tasks.Count == 0) { await System.Threading.Tasks.Task.Delay(500); return; }
            await System.Threading.Tasks.Task.WhenAll(tasks);
        }
    }
}


