using Logic.Scripts.GameDomain.MVC.Abilitys;
using Logic.Scripts.Services.UpdateService;
using Zenject;

public abstract class TargetingStrategy: IUpdatable {
    protected AbilityData Ability;
    protected IEffectable Caster;
    [Inject]
    protected IUpdateSubscriptionService SubscriptionService;

    public virtual void Initialize(AbilityData data, IEffectable caster) {
        Ability = data;
        Caster = caster;
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
