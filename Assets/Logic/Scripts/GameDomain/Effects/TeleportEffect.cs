using Logic.Scripts.GameDomain.MVC.Abilitys;
using UnityEngine;

public class TeleportEffect : AbilityEffect {
    [HideInInspector] public Vector3 _destination;

    public override void SetUp(Vector3 point) {
        base.SetUp(point);
        _destination = point;
    }

    public override void Execute(AbilityData data, IEffectable caster) {
        caster.GetReferenceTransform().position = _destination;
    }
}
