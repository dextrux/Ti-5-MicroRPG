using UnityEngine;
namespace Logic.Scripts.Core.Mvc.WorldCamera {
    public interface IWorldCameraController {
        public void UpdateAngles();
        public void StartFollowTarget(Transform targetTransform);
        public void StopFollowTarget();
        public void UnlockCameraRotate();
        public void LockCameraRotate();
        public void SetMouseDelta(Vector2 delta);
    }
}