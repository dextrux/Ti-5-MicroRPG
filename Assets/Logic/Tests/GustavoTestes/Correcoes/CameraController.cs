using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController
{
    private CameraModel model;

    private bool rotateEnabled;
    private Vector2 mouseDelta;

    public float HorizontalAngle => model.horizontalAngle;
    public float VerticalAngle => model.verticalAngle;

    public CameraController(CameraModel model, GameInputActions inputActions)
    {
        this.model = model;

        inputActions.Camera.RotateButton.performed += ctx => rotateEnabled = true;
        inputActions.Camera.RotateButton.canceled += ctx => rotateEnabled = false;

        inputActions.Camera.RotateCamera.performed += ctx => mouseDelta = ctx.ReadValue<Vector2>();
        inputActions.Camera.RotateCamera.canceled += ctx => mouseDelta = Vector2.zero;

        inputActions.Enable();
    }

    public void UpdateAngles(float deltaTime)
    {
        if (!rotateEnabled) return;

        model.horizontalAngle += mouseDelta.x * model.velocidade * deltaTime;
    }
}
