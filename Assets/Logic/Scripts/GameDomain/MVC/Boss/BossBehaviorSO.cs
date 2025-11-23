using UnityEngine;
using Logic.Scripts.GameDomain.MVC.Abilitys;

namespace Logic.Scripts.GameDomain.MVC.Boss
{
    [CreateAssetMenu(fileName = "BossBehavior", menuName = "Scriptable Objects/BossBehavior")]
    public class BossBehaviorSO : ScriptableObject
    {
		public enum TurnMoveMode { Forward, TowardPlayer, Random, TowardArenaCenter }

        [System.Serializable]
        public struct BossTurnConfig
        {
            public TurnMoveMode Mode;
            public float DistanceMultiplier;
			public int[] AttackIndices; // múltiplos ataques preparados para o mesmo ciclo; use 1 elemento para ataque único
        }

        [Header("Available Attacks")] 
        [SerializeField] private BossAttack[] _availableAttacks;

        [Header("Base Movement Settings")]
        [SerializeField] private float _fallbackStepDistance = 1f;
        [SerializeField] private float _stepDistance = 1f;
        [SerializeField] private float _randomChangeDirectionSeconds = 1.5f;

        [Header("Turn Pattern")]
        [Min(1)][SerializeField] private int _turnPatternLength = 1;
        [SerializeField] private BossTurnConfig[] _turnPattern;

        public BossAttack[] AvailableAttacks => _availableAttacks;
        public float FallbackStepDistance => _fallbackStepDistance;
        public float StepDistance => _stepDistance;
        public float RandomChangeDirectionSeconds => _randomChangeDirectionSeconds;
        public int TurnPatternLength => _turnPatternLength;
        public BossTurnConfig[] TurnPattern => _turnPattern;
    }
}


