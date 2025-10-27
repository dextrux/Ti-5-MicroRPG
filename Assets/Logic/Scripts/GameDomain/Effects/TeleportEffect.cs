using Logic.Scripts.GameDomain.MVC.Abilitys;
using Logic.Scripts.GameDomain.MVC.Nara;
using UnityEngine;

public class TeleportEffect : AbilityEffect {
    public Vector3 _destination;

    public void SetDestination(Vector3 destination) {
        _destination = destination;
    }

    public override void Execute(IEffectable caster, IEffectable target) {
        caster.GetReferenceTransform().position = _destination;
    }

    public override void Execute(AbilityData data, IEffectable caster, IEffectable target) {
        NaraController controller = (NaraController)caster;
        caster.GetReferenceTransform().position = _destination;
    }
}
