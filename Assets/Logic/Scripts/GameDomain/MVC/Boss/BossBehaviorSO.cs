using UnityEngine;
using Logic.Scripts.GameDomain.MVC.Abilitys;

namespace Logic.Scripts.GameDomain.MVC.Boss
{
    [CreateAssetMenu(fileName = "BossBehavior", menuName = "Scriptable Objects/BossBehavior")]
    public class BossBehaviorSO : ScriptableObject
    {
        public enum MoveKind { Forward, TowardPlayer, Offset }

        [System.Serializable]
        public struct BossMoveEntry
        {
            public MoveKind Kind;
            public float Distance;
            public Vector3 Offset;
        }

        [Header("Abilities when HP > 50%")]
        [SerializeField] private AbilityData[] _highHpAbilities;
        [SerializeField] private int[] _highHpDelays; 

        [Header("Abilities when HP <= 50%")]
        [SerializeField] private AbilityData[] _lowHpAbilities;
        [SerializeField] private int[] _lowHpDelays; 

        [Header("Movement Pattern (cycled per boss turn)")]
        [SerializeField] private BossMoveEntry[] _movePattern;
        [SerializeField] private float _fallbackStepDistance = 1f;
        
        [SerializeField] private float _stepDistance = 1f;

        public AbilityData[] HighHpAbilities => _highHpAbilities;
        public int[] HighHpDelays => _highHpDelays;
        public AbilityData[] LowHpAbilities => _lowHpAbilities;
        public int[] LowHpDelays => _lowHpDelays;
        public BossMoveEntry[] MovePattern => _movePattern;
        public float FallbackStepDistance => _fallbackStepDistance;
        public float StepDistance => _stepDistance;
    }
}


