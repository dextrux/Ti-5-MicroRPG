using UnityEngine;

namespace Logic.Scripts.Core.Mvc.UICamera {
    public class UICameraView : MonoBehaviour {
        [field: SerializeField] public Camera Camera { get; private set; }
    }
}
