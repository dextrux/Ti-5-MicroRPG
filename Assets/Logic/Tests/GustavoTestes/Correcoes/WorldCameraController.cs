using UnityEngine;
using UnityEngine.InputSystem;

public class WorldCameraController
{
    private CameraData data;

    private bool rotateEnabled;
    private Vector2 mouseDelta;

    private Transform target;

    public float HorizontalAngle => data.horizontalAngle;
    public float VerticalAngle => data.verticalAngle;

    public WorldCameraController(WorldCameraView worldCameraView, CameraData worldCameradata, GameInputActions inputActions)
    {
        this.data = data;

        inputActions.Camera.RotateButton.performed += ctx => rotateEnabled = true;
        inputActions.Camera.RotateButton.canceled += ctx => rotateEnabled = false;

        inputActions.Camera.RotateCamera.performed += ctx => mouseDelta = ctx.ReadValue<Vector2>();
        inputActions.Camera.RotateCamera.canceled += ctx => mouseDelta = Vector2.zero;

        inputActions.Enable();
    }

    public void UpdateAngles(float deltaTime)
    {
        if (!rotateEnabled) return;

        data.horizontalAngle += mouseDelta.x * data.velocidade * deltaTime;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        
    }

    public Transform GetTarget()
    {
        return target;
    }
}
