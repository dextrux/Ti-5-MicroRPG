using UnityEngine;

namespace Logic.Scripts.GameDomain.MVC.Nara {
    public interface INaraController {
        GameObject NaraViewGO { get; }
        Transform NaraSkillSpotTransform { get; }
        NaraMovementController NaraMove { get; }
        void InitEntryPoint();
        void CreateNara();
        void RegisterListeners();
        void ManagedFixedUpdate();
        void PlayAttackType(int type);
        void PlayAttackType1();
        void TriggerExecute();
        void ResetExecuteTrigger();
        void SetPosition(Vector3 movementCenter);
    }
}
