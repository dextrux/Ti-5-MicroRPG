using Logic.Scripts.GameDomain.MVC.Nara;
using System;
using UnityEngine;

public class PortalView : MonoBehaviour {
    [SerializeField] private int _levelIndex;

    private Action<PortalView, int> _onTriggerEnterAction;

    public void Setup(Action<PortalView, int> OnStepInside) {
        _onTriggerEnterAction = OnStepInside;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.TryGetComponent<NaraView>(out NaraView naraView)) {
            _onTriggerEnterAction?.Invoke(this, _levelIndex);
        }
    }
}
