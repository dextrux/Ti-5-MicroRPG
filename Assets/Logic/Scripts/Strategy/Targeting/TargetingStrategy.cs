using Logic.Scripts.GameDomain.MVC.Abilitys;
using Logic.Scripts.Services.UpdateService;
using UnityEngine;

public abstract class TargetingStrategy : IUpdatable {
    protected AbilityData Ability;
    protected IEffectable Caster;
    protected IUpdateSubscriptionService SubscriptionService;
    protected LayerMask CastableLayerMask;
    public virtual void Initialize(AbilityData data, IEffectable caster) {
        Ability = data;
        Caster = caster;
    }

    public virtual void SetUp(IUpdateSubscriptionService updateSubscriptionService) {
        SubscriptionService = updateSubscriptionService;
    }

    public virtual Vector3 LockAim(out IEffectable[] targets) {
        targets = null;
        return Vector3.zero;
    }
    public virtual void Cancel() {
        Ability = null;
        SubscriptionService.UnregisterUpdatable(this);
    }

    protected Vector3 GetMousePosition() {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, float.MaxValue, CastableLayerMask)) {
            return hit.point;
        }
        else {
            return Vector3.zero;
        }
    }

    public virtual void ManagedUpdate() {

    }
}
