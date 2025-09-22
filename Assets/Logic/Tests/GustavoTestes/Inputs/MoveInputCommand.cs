using Logic.Scripts.Services.CommandFactory;
using Logic.Scripts.Services.Logger.Base;
using Logic.Scripts.GameDomain.MVC.Nara;
using UnityEngine;

public class MoveInputCommand : BaseCommand, ICommandVoid
{
    private GameInputActions _gameInputActions;
    private INaraController _naraController;

    private const float MoveSpeed = 10f;
    private const float RotationSpeed = 10f;

    public override void ResolveDependencies()
    {
        _naraController = _diContainer.Resolve<INaraController>();
        _gameInputActions = _diContainer.Resolve<GameInputActions>();
    }

    public void Execute()
    {
        Vector2 dir = _gameInputActions.Player.Move.ReadValue<Vector2>();
        _naraController.NaraMove.CheckRadiusLimit();
        _naraController.NaraMove.Move(dir, MoveSpeed, RotationSpeed);
    }
}
