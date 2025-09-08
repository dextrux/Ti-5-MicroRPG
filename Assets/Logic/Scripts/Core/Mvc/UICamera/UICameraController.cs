using UnityEngine;

namespace Logic.Scripts.Core.Mvc.UICamera {
    public class UICameraController : IUICameraController {
        private readonly UICameraView _uiCameraView;

        public Camera UICamera => _uiCameraView.Camera;

        public UICameraController(UICameraView uiCameraView) {
            _uiCameraView = uiCameraView;
        }
    }
}
