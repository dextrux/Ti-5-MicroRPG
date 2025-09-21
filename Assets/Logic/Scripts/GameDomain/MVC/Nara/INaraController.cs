using UnityEngine;

namespace Logic.Scripts.GameDomain.MVC.Nara {
    public interface INaraController {

        GameObject NaraViewGO { get; }

        void InitEntryPoint();
        void CreateNara();
        void ResetController();
        void RegisterListeners();
        void DisableCallbacks();
    }
}
