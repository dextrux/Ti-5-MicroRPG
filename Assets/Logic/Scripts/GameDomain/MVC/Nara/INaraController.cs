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
        void ManagedFixedUpdate();
        public void ExecuteAbility(AbilityData abilityData, IEffectable castter);
        void RecenterMovementAreaAtTurnStart();
        void SetMovementCircleVisible(bool visible);
        void PlayAttackType(int type);
        void PlayAttackType1();
        void TriggerExecute();
        void ResetExecuteTrigger();
    }
}
