using Logic.Scripts.GameDomain.MVC.Abilitys;
using Logic.Scripts.Services.UpdateService;
using UnityEngine;
using Zenject;

public abstract class ProjectileController : MonoBehaviour, IFixedUpdatable {
    [field: SerializeField] public float InitialSpeed { get; protected set; }
    [field: SerializeField] public Rigidbody GetRigidbody { get; protected set; }
    protected IEffectable Caster;
    protected AbilityData Data;
    [Inject]
    private IUpdateSubscriptionService _subscriptionService;

    public virtual void Initialize(Transform castTransform, IEffectable caster, AbilityData data) {
        Caster = caster;
        Data = data;
    }

    private void RegisterOnUpdate() {
        _subscriptionService.RegisterFixedUpdatable(this);
    }

    private void UnregisterOnUpdate() {
        _subscriptionService.UnregisterFixedUpdatable(this);
    }

    public abstract void ManagedFixedUpdate();

    private void OnTriggerEnter(Collider other) {
        if (other) OnHit();
    }

    public void OnHit() {
        UnregisterOnUpdate();
        Destroy(this);
    }

}
