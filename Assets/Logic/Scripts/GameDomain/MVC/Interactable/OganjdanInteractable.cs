using UnityEngine;
using Zenject;

public class OganjdanInteractable : InteractableObjects
{
    [Inject]
    public override void OnInteract() {
        Debug.LogWarning("Oganjdan Interact");
        //CommandFactory.CreateCommandVoid<>;
    }
}
