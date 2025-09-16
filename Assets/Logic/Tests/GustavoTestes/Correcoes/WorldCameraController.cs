using UnityEngine;

public class WorldCameraController {
    private CameraData data;

    private bool rotateEnabled;
    private Vector2 mouseDelta;

    private Transform target;

    public float HorizontalAngle => data.horizontalAngle;
    public float VerticalAngle => data.verticalAngle;

    public WorldCameraController(WorldCameraView worldCameraView, CameraData worldCameradata) {
    }

    public void UpdateAngles(float deltaTime) {
        if (!rotateEnabled) return;

        data.horizontalAngle += mouseDelta.x * data.velocidade * deltaTime;
    }

    public void SetTarget(Transform newTarget) {
        target = newTarget;

    }

    public Transform GetTarget() {
        return target;
    }
}
