using Logic.Scripts.Extensions;
using Logic.Scripts.GameDomain.MVC.Nara;
using Logic.Scripts.Services.CommandFactory;

public class InteractableObjectsController : IInteractableObjectsController {
    private InteractableObjects[] _interactablesViews;
    private ICommandFactory _commandFactory;

    public InteractableObjects[] InteractableObjects => _interactablesViews;

    public InteractableObjectsController(ICommandFactory commandFactory) {
        _commandFactory = commandFactory;
    }

    public void SetUpInteractables(InteractableObjects[] interactablesViews) {
        _interactablesViews = interactablesViews;
    }

    public InteractableObjects VerifyInteractables(INaraController naraController) {
        if (_interactablesViews.IsNullOrEmpty()) {
            return null;
        }
        foreach (InteractableObjects interactable in _interactablesViews) {
            if (interactable.CanInteract(naraController, _commandFactory)) {
                return interactable;
            }
        }
        return null;
    }
}
