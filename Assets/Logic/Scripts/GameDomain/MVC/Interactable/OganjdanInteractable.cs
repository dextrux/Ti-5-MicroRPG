using Logic.Scripts.Services.CommandFactory;
using UnityEngine;
using Zenject;

public class OganjdanInteractable : InteractableObjects
{
    public override void OnInteract() {
        Debug.LogWarning("Oganjdan Interact");
    }
}
