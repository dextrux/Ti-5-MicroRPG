using Logic.Scripts.GameDomain.MVC.Abilitys;
using Logic.Scripts.Services.UpdateService;

public abstract class TargetingStrategy: IUpdatable {
    protected AbilityData Ability;
    protected IEffectable Caster;
    protected IUpdateSubscriptionService SubscriptionService;
    public virtual void Initialize(AbilityData data, IEffectable caster) {
        Ability = data;
        Caster = caster;
    }

    public virtual void SetUp(IUpdateSubscriptionService updateSubscriptionService) {
        SubscriptionService = updateSubscriptionService;
    }

    protected virtual void LockAim(out IEffectable target) {
        target = null;
    }
    public virtual void LockAim(out IEffectable[] targets) {
        targets = null;
    }
    public virtual void Cancel() {
        Ability = null;
        SubscriptionService.UnregisterUpdatable(this);
    }

    public virtual void ManagedUpdate() {

    }
}
