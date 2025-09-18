using UnityEngine;

namespace Logic.Scripts.GameDomain.MVC.Nara {
    [CreateAssetMenu(fileName = "NaraConfiguration", menuName = "Scriptable Objects/NaraConfiguration")]
    public class NaraConfigurationSO : ScriptableObject {
        [field: SerializeField] public int MaxHealth { get; private set; }
        [field: SerializeField] public int InitialMovementDistance { get; private set; }
        [field: SerializeField] public int MaxActionPoints { get; private set; }
        [field: SerializeField] public int ActionPointsTurnGain { get; private set; }
    }
}
