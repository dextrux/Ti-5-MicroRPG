using Logic.Scripts.Services.UpdateService;
using UnityEngine;

public class WorldCameraController: IUpdatable {

    private bool _rotateEnabled;
    private Vector2 _mouseDelta;
    private Transform _target;
    private readonly WorldCameraView _worldCameraView;
    private readonly IUpdateSubscriptionService _updateSubscriptionService;

    public WorldCameraController(WorldCameraView worldCameraView) {
        _worldCameraView = worldCameraView;
    }

    public void UpdateAngles() {
        if (!_rotateEnabled) return;
        _worldCameraView.UpdateCameraRotation(_mouseDelta.x, Time.deltaTime);
    }

    public void StartFollowTarget (Transform targetTransform) {
        _target = targetTransform;
        _worldCameraView.SetNewTarget(_target);
        _updateSubscriptionService.RegisterUpdatable(this);
    }

    public void StopFollowTarget() {
        _updateSubscriptionService.UnregisterUpdatable(this);
        _target = null;
    }

    public void ManagedUpdate() {
        UpdateAngles();
    }
}
