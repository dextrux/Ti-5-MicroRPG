using UnityEngine;

namespace Logic.Scripts.GameDomain.MVC.Boss
{
    [CreateAssetMenu(fileName = "BossConfiguration", menuName = "Scriptable Objects/BossConfiguration")]
    public class BossConfigurationSO : ScriptableObject
    {
        [field: SerializeField] public int MaxHealth { get; private set; }
        [field: SerializeField] public int InitialMovementDistance { get; private set; }
    }
}
