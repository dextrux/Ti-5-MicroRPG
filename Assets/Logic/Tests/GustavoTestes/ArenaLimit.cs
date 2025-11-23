using UnityEngine;
using Zenject;
using Logic.Scripts.Services.UpdateService;
using Logic.Scripts.Turns;

[RequireComponent(typeof(MeshCollider))]
public class ArenaLimit : MonoBehaviour, IUpdatable
{
    private ITurnStateReader _turnReader;
    private IUpdateSubscriptionService _updateService;
    private MeshCollider _meshCollider;
    private bool _lastApplied;

    [Inject]
    public void Construct(ITurnStateReader turnReader, IUpdateSubscriptionService updateSvc)
    {
        _turnReader = turnReader;
        _updateService = updateSvc;

        _meshCollider = GetComponent<MeshCollider>();
        if (_meshCollider == null) return;

        _updateService.RegisterUpdatable(this);
        ApplyNow();
    }

    private void OnDestroy()
    {
        _updateService?.UnregisterUpdatable(this);
    }

    public void ManagedUpdate()
    {
        if (_meshCollider == null || _turnReader == null) return;

        bool shouldEnable = _turnReader.Active && _turnReader.Phase == TurnPhase.PlayerAct;
        if (shouldEnable != _lastApplied)
        {
            _meshCollider.enabled = shouldEnable;
            _lastApplied = shouldEnable;
        }
    }

    private void ApplyNow()
    {
        if (_meshCollider == null || _turnReader == null) return;
        _lastApplied = _turnReader.Active && _turnReader.Phase == TurnPhase.PlayerAct;
        _meshCollider.enabled = _lastApplied;
    }
}
