using Logic.Scripts.Services.UpdateService;
using UnityEngine;
namespace Logic.Scripts.Core.Mvc.WorldCamera {
    public interface IWorldCameraController {
        bool IsRotateEnabled { get; }
        public void UpdateAngles();
        public void StartFollowTarget(Transform targetTransform, IUpdateSubscriptionService updateSubscriptionService);
        public void StopFollowTarget();
        public void UnlockCameraRotate();
        public void LockCameraRotate();
        public void SetMouseDelta(Vector2 delta);
    }
}