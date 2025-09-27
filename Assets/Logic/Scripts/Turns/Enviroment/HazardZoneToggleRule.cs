using System.Collections.Generic;

namespace Logic.Scripts.Turns
{
    public class HazardZoneToggleRule : IEnviromentRule
    {
        private readonly TurnStateService _turnStateService;
        private readonly Dictionary<string, bool> _zoneIdToHazard;

        public HazardZoneToggleRule(TurnStateService turnStateService)
        {
            _turnStateService = turnStateService;
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
            List<HazardZoneState> zones = new List<HazardZoneState>();
            foreach (KeyValuePair<string, bool> kv in _zoneIdToHazard)
            {
                zones.Add(new HazardZoneState { Id = kv.Key, IsHazard = kv.Value });
            }
            _turnStateService.PublishHazardZonesChanged(zones);
        }
    }
}
