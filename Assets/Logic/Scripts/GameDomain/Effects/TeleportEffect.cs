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
            controller = (INaraController)caster;
            controller.NaraMove.RecalculateRadiusAfterAbility();
            int naraRadius = controller.NaraMove.GetNaraRadius();
            controller.NaraMove.RemoveMovementRadius();
            caster.GetReferenceTransform().position = _destination;
            controller.NaraMove.SetNaraRadius(naraRadius);
            controller.NaraMove.SetMovementRadiusCenter();
        }
        else {
            caster.GetReferenceTransform().position = _destination;
        }

    }
}
