using Logic.Scripts.Services.UpdateService;
using UnityEngine;
using Zenject;

public abstract class ProjectileController : MonoBehaviour, IFixedUpdatable {
    [SerializeField] protected float Speed;
    protected Transform CastTransform;
    [Inject]
    private IUpdateSubscriptionService _subscriptionService;

    public virtual void Initialize(Transform castTransform) {
        RegisterOnUpdate();
        CastTransform = castTransform;
    }

    private void RegisterOnUpdate() {
        _subscriptionService.RegisterFixedUpdatable(this);
    }

    private void UnregisterOnUpdate() {
        _subscriptionService.UnregisterFixedUpdatable(this);
    }

    public abstract void ManagedFixedUpdate();

    private void OnTriggerEnter(Collider other) {
        OnHit();
    }

    public void OnHit() {
        UnregisterOnUpdate();
        Destroy(this);
    }

}
