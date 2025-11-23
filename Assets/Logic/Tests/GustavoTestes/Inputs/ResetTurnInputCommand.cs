using Logic.Scripts.Services.CommandFactory;
using Logic.Scripts.Services.Logger.Base;
using Logic.Scripts.GameDomain.MVC.Nara;

public class ResetTurnInputCommand : BaseCommand, ICommandVoid {
    private GameInputActions _gameInputActions;
    private INaraController _naraController;

    public override void ResolveDependencies() {
        _naraController = _diContainer.Resolve<INaraController>();
        _gameInputActions = _diContainer.Resolve<GameInputActions>();
    }

    public void Execute() {
        if (_naraController?.NaraMove is NaraTurnMovementController naraTurnMovement) _naraController.SetPosition(naraTurnMovement.GetNaraCenter());
    }
}
