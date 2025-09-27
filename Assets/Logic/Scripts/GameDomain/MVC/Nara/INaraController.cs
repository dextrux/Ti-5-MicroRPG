using Logic.Scripts.GameDomain.MVC.Ui;
using Logic.Scripts.GameDomain.MVC.Abilitys;
using UnityEngine;

namespace Logic.Scripts.GameDomain.MVC.Nara {
    public interface INaraController {
        GameObject NaraViewGO { get; }
        Transform NaraSkillSpotTransform { get; }
        NaraMovementController NaraMove { get; }
        void InitEntryPoint();
        void CreateNara();
        void ResetController();
        void RegisterListeners();
        void DisableCallbacks();
        void ManagedFixedUpdate();
        public void ExecuteAbility(AbilityData abilityData, IEffectable castter);
    }
}
