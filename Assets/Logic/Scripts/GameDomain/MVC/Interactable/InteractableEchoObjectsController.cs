using Logic.Scripts.Extensions;
using Logic.Scripts.GameDomain.MVC.Echo;
using Logic.Scripts.GameDomain.MVC.Nara;

public class InteractableEchoObjectsController : IInteractableEchoObjectsController {
    private InteractableEchoObjects[] _interactablesEchoViews;

    public InteractableEchoObjects[] InteractableEchoObjects => _interactablesEchoViews;

    public void SetUpInteractables(InteractableEchoObjects[] interactablesEchoViews) {
        _interactablesEchoViews = interactablesEchoViews;
    }

    public InteractableEchoObjects VerifyInteractables(INaraController naraController, EchoView echoView) {
        if (_interactablesEchoViews.IsNullOrEmpty()) {
            return null;
        }
        foreach (InteractableEchoObjects interactable in _interactablesEchoViews) {
            if (interactable.CanInteract(naraController, echoView)) {
                return interactable;
            }
        }
        return null;
    }
}