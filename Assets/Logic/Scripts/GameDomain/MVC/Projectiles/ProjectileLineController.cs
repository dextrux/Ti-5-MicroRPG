using Logic.Scripts.GameDomain.MVC.Abilitys;
using UnityEngine;

public class ProjectileLineController : ProjectileController {
    public override void ManagedFixedUpdate() {
    }

    private void OnTriggerEnter(Collider other) {
        if (other.TryGetComponent<IEffectable>(out IEffectable target)) {
            foreach (AbilityEffect effect in Data.Effects) {
                effect.Execute(Data, Caster, target);
            }
        }
    }
}
