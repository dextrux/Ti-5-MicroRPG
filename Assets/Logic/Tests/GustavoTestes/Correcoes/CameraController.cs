using UnityEngine;

public class CameraController
{
    private CameraModel model;

    public float HorizontalAngle => model.horizontalAngle;
    public float VerticalAngle => model.verticalAngle;

    public CameraController(CameraModel model)
    {
        this.model = model;
    }

    public void UpdateAngles(float deltaTime)
    {

        //Teste antes de implementar inputsys
        float inputH = 0f;
        float inputV = 0f;

        if (Input.GetKey(KeyCode.W)) inputV = 1f;
        if (Input.GetKey(KeyCode.S)) inputV = -1f;

        model.horizontalAngle += inputH * model.velocidade * deltaTime;
        model.verticalAngle += inputV * model.velocidade * deltaTime;
    }
}

