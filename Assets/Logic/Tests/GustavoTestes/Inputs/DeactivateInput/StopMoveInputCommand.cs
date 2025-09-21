using Logic.Scripts.Services.CommandFactory;
using Logic.Scripts.GameDomain.MVC.Nara;
using UnityEngine;

public class StopMoveInputCommand : BaseCommand, ICommandVoid
{
    private INaraController _naraController;

    public override void ResolveDependencies()
    {
        _naraController = _diContainer.Resolve<INaraController>();
    }

    public void Execute()
    {
        _naraController.NaraMove.Move(Vector2.zero, 0f, 0f);
    }
}
