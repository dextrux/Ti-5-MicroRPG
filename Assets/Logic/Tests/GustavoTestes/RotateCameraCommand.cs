using UnityEngine;
using Zenject;

public class RotateCameraCommand
{
    private readonly ICameraController _cameraController;

    [Inject]
    public RotateCameraCommand(ICameraController cameraController)
    {
        _cameraController = cameraController;
    }

    public void Execute(Vector2 input)
    {
        _cameraController.Rotate(input);
    }
}
