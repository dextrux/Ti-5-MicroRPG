using Logic.Scripts.Services.UpdateService;
using UnityEngine;

namespace Logic.Scripts.Core.Mvc.WorldCamera {
    public class WorldCameraController : IUpdatable, IWorldCameraController {

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

        public void StartFollowTarget(Transform targetTransform) {
            _target = targetTransform;
            _worldCameraView.SetNewTarget(_target);
            _updateSubscriptionService.RegisterUpdatable(this);
        }

        public void StopFollowTarget() {
            _updateSubscriptionService.UnregisterUpdatable(this);
            _target = null;
        }

        public void UnlockCameraRotate() {
            _rotateEnabled = true;
        }

        public void LockCameraRotate() {
            _rotateEnabled = false;
        }

        public void ManagedUpdate() {
            UpdateAngles();
        }
    }
}