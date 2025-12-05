using Logic.Scripts.GameDomain.GameInputActions;
using Logic.Scripts.GameDomain.MVC.Nara;
using Logic.Scripts.Services.CommandFactory;

public class InteractInputCommand : BaseCommand, ICommandVoid {
    private IInteractableObjectsController _interactableObjectsController;
    private IGameInputActionsController _InputActionsController;
    private INaraController _naraController;
    public override void ResolveDependencies() {
        _interactableObjectsController = _diContainer.Resolve<IInteractableObjectsController>();
        _InputActionsController = _diContainer.Resolve<IGameInputActionsController>();
        _naraController = _diContainer.Resolve<INaraController>();
    }

    public void Execute() {
        _interactableObjectsController.VerifyInteractables(_naraController);
    }
}
