using Logic.Scripts.GameDomain.MVC.Abilitys;
using UnityEngine;

public class AbilityView : MonoBehaviour {
    [field: SerializeField] public AbilityData AbilityData { get; private set; }
    [SerializeField] private float timeToDestoy;

    private void Awake() {
        Destroy(this, timeToDestoy);
    }
}
