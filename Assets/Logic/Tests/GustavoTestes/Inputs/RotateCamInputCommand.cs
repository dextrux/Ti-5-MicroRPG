using Logic.Scripts.Services.CommandFactory;
using Logic.Scripts.Core.Mvc.WorldCamera;
using UnityEngine;

public class RotateCamInputCommand : BaseCommand, ICommandVoid {
    private IWorldCameraController _WorldCameraController;
    private GameInputActions _gameInputActions;

    public override void ResolveDependencies() {
        _WorldCameraController = _diContainer.Resolve<IWorldCameraController>();
        _gameInputActions = _diContainer.Resolve<GameInputActions>();
    }

    public void Execute() {
        if (_WorldCameraController.IsRotateEnabled) {
            Vector2 delta = Vector2.zero;
            if (_gameInputActions.Player.enabled == true) delta = _gameInputActions.Player.RotateCam.ReadValue<Vector2>();
            if (_gameInputActions.Exploration.enabled == true) delta = _gameInputActions.Exploration.RotateCam.ReadValue<Vector2>();
            _WorldCameraController.SetMouseDelta(delta);
        }
    }
}
