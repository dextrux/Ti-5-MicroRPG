using Logic.Scripts.GameDomain.MVC.Abilitys;
using Logic.Scripts.GameDomain.MVC.Nara;
using UnityEngine;

public class TeleportEffect : AbilityEffect {
    [HideInInspector] public Vector3 _destination;

    public override void SetUp(Vector3 point) {
        base.SetUp(point);
        _destination = point;
    }

    public override void Execute(AbilityData data, IEffectable caster) {
        if (caster is INaraController controller) {
            if (controller.NaraMove is NaraTurnMovementController turnMovement) {
                turnMovement.RecalculateRadiusAfterAbility();
                int naraRadius = turnMovement.GetNaraRadius();
                turnMovement.RemoveMovementRadius();
                caster.GetReferenceTransform().position = _destination;
                turnMovement.SetNaraRadius(naraRadius);
                turnMovement.SetMovementRadiusCenter();
            }
        }
        else {
            caster.GetReferenceTransform().position = _destination;
        }

    }
}
