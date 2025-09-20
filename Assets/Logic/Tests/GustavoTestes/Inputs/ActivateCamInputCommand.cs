using Logic.Scripts.Services.CommandFactory;
using Logic.Scripts.Core.Mvc.WorldCamera;
using UnityEngine;

public class ActivateCamInputCommand : BaseCommand, ICommandVoid
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
        if (_gameInputActions.Player.ActivateCam.IsPressed())
        {
            _iWorldCameraController.UnlockCameraRotate();
        }
        else
        {
            _iWorldCameraController.LockCameraRotate();
        }
    }
}
