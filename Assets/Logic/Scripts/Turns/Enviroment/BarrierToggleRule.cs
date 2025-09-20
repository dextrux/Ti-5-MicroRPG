using System.Collections.Generic;

namespace Logic.Scripts.Turns
{
    public class BarrierToggleRule : IEnviromentRule
    {
        private readonly ITurnEventBus _eventBus;
        private readonly Dictionary<string, bool> _barrierIdToActive;

        public BarrierToggleRule(ITurnEventBus eventBus)
        {
            _eventBus = eventBus;
            _barrierIdToActive = new Dictionary<string, bool>();
        }

        public void ConfigureInitialBarriers(IEnumerable<string> barrierIds, bool startActive)
        {
            foreach (string id in barrierIds)
            {
                _barrierIdToActive[id] = startActive;
            }
            PublishState();
        }

        public void Execute()
        {
            List<string> keys = new List<string>(_barrierIdToActive.Keys);
            for (int i = 0; i < keys.Count; i++)
            {
                string id = keys[i];
                bool current = _barrierIdToActive[id];
                _barrierIdToActive[id] = !current;
            }
            PublishState();
        }

        private void PublishState()
        {
            BarrierStateChangedSignal s = new BarrierStateChangedSignal
            {
                Barriers = new List<BarrierState>()
            };
            foreach (System.Collections.Generic.KeyValuePair<string, bool> kv in _barrierIdToActive)
            {
                s.Barriers.Add(new BarrierState { Id = kv.Key, IsActive = kv.Value });
            }
            _eventBus.Publish(s);
        }
    }
}
