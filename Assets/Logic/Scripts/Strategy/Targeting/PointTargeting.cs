using Logic.Scripts.GameDomain.MVC.Abilitys;
using UnityEngine;

public class PointTargeting : TargetingStrategy {
    [SerializeField] private AbilitySummon _objectToSummon;
    [SerializeField] private AbilitySummon _needLookPlayer;
    private Transform _previewTransform;
    public override void Initialize(AbilityData data, IEffectable caster) {
        base.Initialize(data, caster);
        SubscriptionService.RegisterUpdatable(this);
        _previewTransform = Object.Instantiate(_objectToSummon.VisualRoot).transform;
    }
    public override void ManagedUpdate() {
        base.ManagedUpdate();
        if (_previewTransform != null) {
            _previewTransform.transform.position = GetMousePosition();

            Vector3 directionToLook = _previewTransform.transform.position - Caster.GetReferenceTransform().position;
            Vector3 previewDirectionToLook = Caster.GetReferenceTransform().position - _previewTransform.transform.position;

            if (directionToLook != Vector3.zero) {
                Caster.GetReferenceTransform().rotation = Quaternion.LookRotation(directionToLook);
                _previewTransform.rotation = Quaternion.LookRotation(directionToLook);
            }
        }
    }

    public override Vector3 LockAim(out IEffectable[] targets) {
        base.LockAim(out targets);
        Object.Instantiate(_objectToSummon, _previewTransform.position, _previewTransform.rotation);
        return _previewTransform.position;
    }

    public override void Cancel() {
        base.Cancel();
    }
}
