using Logic.Scripts.GameDomain.MVC.Echo;
using Logic.Scripts.GameDomain.MVC.Nara;

public interface IInteractableEchoObjectsController {
    InteractableEchoObjects[] InteractableEchoObjects { get; }
    void SetUpInteractables(InteractableEchoObjects[] interactablesEchoViews);
    InteractableEchoObjects VerifyInteractables(INaraController naraController, EchoView echoView);
}
