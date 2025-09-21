using Logic.Scripts.Services.CommandFactory;
using Logic.Scripts.Core.Mvc.WorldCamera;
using UnityEngine;

public class RotateCamInputCommand : BaseCommand, ICommandVoid
{
    private IWorldCameraController _iWorldCameraController;
    private GameInputActions _gameInputActions;

    public override void ResolveDependencies()
    {
        _iWorldCameraController = _diContainer.Resolve<IWorldCameraController>();
        _gameInputActions = _diContainer.Resolve<GameInputActions>();
    }

    public void Execute()
    {
        Vector2 delta = _gameInputActions.Player.RotateCam.ReadValue<Vector2>();
        _iWorldCameraController.SetMouseDelta(delta);
    }
}
