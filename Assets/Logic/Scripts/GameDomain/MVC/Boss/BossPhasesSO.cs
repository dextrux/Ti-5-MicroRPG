using UnityEngine;

namespace Logic.Scripts.GameDomain.MVC.Boss
{
    [CreateAssetMenu(fileName = "BossPhases", menuName = "Scriptable Objects/BossPhases")]
    public class BossPhasesSO : ScriptableObject
    {
        public enum PhaseTriggerType { HealthPercentBelow, HealthAbsoluteBelow }

        [System.Serializable]
        public struct PhaseEntry
        {
            public string Name;
            public PhaseTriggerType TriggerType;
            [Range(0f, 1f)] public float HealthPercentThreshold;
            public int HealthAbsoluteThreshold;
            public BossBehaviorSO Behavior;
        }

        [SerializeField] private PhaseEntry[] _phases;
        public PhaseEntry[] Phases => _phases;

        public int GetPhaseIndexByHealth(int currentHealth, int maxHealth)
        {
            if (_phases == null || _phases.Length == 0) return -1;
            float hpPct = maxHealth > 0 ? Mathf.Clamp01((float)currentHealth / maxHealth) : 0f;
            int selectedIndex = -1;
            float bestThresholdPct = -1f;
            int bestAbsolute = int.MinValue;

            for (int i = 0; i < _phases.Length; i++)
            {
                var p = _phases[i];
                switch (p.TriggerType)
                {
                    case PhaseTriggerType.HealthPercentBelow:
                        if (hpPct <= p.HealthPercentThreshold && p.Behavior != null)
                        {
                            if (p.HealthPercentThreshold > bestThresholdPct)
                            {
                                bestThresholdPct = p.HealthPercentThreshold;
                                selectedIndex = i;
                            }
                        }
                        break;
                    case PhaseTriggerType.HealthAbsoluteBelow:
                        if (currentHealth <= p.HealthAbsoluteThreshold && p.Behavior != null)
                        {
                            if (p.HealthAbsoluteThreshold > bestAbsolute)
                            {
                                bestAbsolute = p.HealthAbsoluteThreshold;
                                selectedIndex = i;
                            }
                        }
                        break;
                }
            }
            // If no phase matched, default to first phase (initial)
            return selectedIndex >= 0 ? selectedIndex : 0;
        }
    }
}


