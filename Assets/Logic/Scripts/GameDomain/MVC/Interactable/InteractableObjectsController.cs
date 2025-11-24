using Logic.Scripts.Extensions;
using Logic.Scripts.GameDomain.MVC.Nara;

public class InteractableObjectsController : IInteractableObjectsController {
    private InteractableObjects[] _interactablesViews;

    public InteractableObjects[] InteractableObjects => _interactablesViews;

    public void SetUpInteractables(InteractableObjects[] interactablesViews) {
        _interactablesViews = interactablesViews;
    }

    public InteractableObjects VerifyInteractables(INaraController naraController) {
        if (_interactablesViews.IsNullOrEmpty()) {
            return null;
        }
        foreach (InteractableObjects interactable in _interactablesViews) {
            if (interactable.CanInteract(naraController)) {
                return interactable;
            }
        }
        return null;
    }
}
