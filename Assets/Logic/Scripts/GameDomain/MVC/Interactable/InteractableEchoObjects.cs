using Logic.Scripts.GameDomain.MVC.Echo;
using Logic.Scripts.GameDomain.MVC.Nara;
using UnityEngine;

public abstract class InteractableEchoObjects : MonoBehaviour {
    [SerializeField] protected float InteractDistance;

    public bool CanInteract(INaraController naraController, EchoView echoView) {
        if (Vector3.Distance(naraController.NaraViewGO.transform.position, transform.position) <= InteractDistance) {
            OnInteract(echoView);
            return true;
        }
        return false;
    }

    public abstract void OnInteract(EchoView echoView);

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, InteractDistance);
    }
}
