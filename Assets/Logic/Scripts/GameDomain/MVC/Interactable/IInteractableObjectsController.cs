using Logic.Scripts.GameDomain.MVC.Nara;

public interface IInteractableObjectsController {
    InteractableObjects[] InteractableObjects { get; }
    void SetUpInteractables(InteractableObjects[] interactablesViews);
    public InteractableObjects VerifyInteractables(INaraController naraController);
}
