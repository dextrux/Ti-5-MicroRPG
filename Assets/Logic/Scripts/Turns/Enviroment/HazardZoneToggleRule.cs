using System.Collections.Generic;

namespace Logic.Scripts.Turns
{
    public class HazardZoneToggleRule : IEnviromentRule
    {
        private readonly ITurnEventBus _eventBus;
        private readonly Dictionary<string, bool> _zoneIdToHazard;

        public HazardZoneToggleRule(ITurnEventBus eventBus)
        {
            _eventBus = eventBus;
            _zoneIdToHazard = new Dictionary<string, bool>();
        }

        public void ConfigureInitialZones(IEnumerable<string> zoneIds, bool startHazard)
        {
            foreach (string id in zoneIds)
            {
                _zoneIdToHazard[id] = startHazard;
            }
            PublishState();
        }

        public void Execute()
        {
            List<string> keys = new List<string>(_zoneIdToHazard.Keys);
            for (int i = 0; i < keys.Count; i++)
            {
                string id = keys[i];
                bool current = _zoneIdToHazard[id];
                _zoneIdToHazard[id] = !current;
            }
            PublishState();
        }

        private void PublishState()
        {
            HazardZonesChangedSignal s = new HazardZonesChangedSignal
            {
                Zones = new List<HazardZoneState>()
            };
            foreach (KeyValuePair<string, bool> kv in _zoneIdToHazard)
            {
                s.Zones.Add(new HazardZoneState { Id = kv.Key, IsHazard = kv.Value });
            }
            _eventBus.Publish(s);
        }
    }
}
