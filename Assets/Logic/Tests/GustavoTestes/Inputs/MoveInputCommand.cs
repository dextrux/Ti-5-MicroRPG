using Logic.Scripts.Services.CommandFactory;
using Logic.Scripts.GameDomain.MVC.Nara;

public class MoveInputCommand : BaseCommand, ICommandVoid {
    private INaraController _naraController;

    public override void ResolveDependencies() {
        _naraController = _diContainer.Resolve<INaraController>();
    }

    public void Execute() {
        _naraController.RegisterListeners();
        if (_naraController.NaraMove is NaraTurnMovementController naraTurnMovement) naraTurnMovement.CheckRadiusLimit();
    }
}
