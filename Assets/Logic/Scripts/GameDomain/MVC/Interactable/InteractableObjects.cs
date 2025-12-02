using Logic.Scripts.GameDomain.MVC.Nara;
using Logic.Scripts.Services.CommandFactory;
using UnityEngine;

public abstract class InteractableObjects : MonoBehaviour {
    [SerializeField] protected float InteractDistance;
    protected ICommandFactory CommandFactory;

    public bool CanInteract(INaraController naraController, ICommandFactory commandFactory) {
        CommandFactory = commandFactory;
        if (Vector3.Distance(naraController.NaraViewGO.transform.position, transform.position) <= InteractDistance) {
            OnInteract();
            return true;
        }
        return false;
    }

    public abstract void OnInteract();

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, InteractDistance);
    }
}
