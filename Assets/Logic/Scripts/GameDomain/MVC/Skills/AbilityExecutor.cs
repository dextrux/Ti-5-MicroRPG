using UnityEngine;

namespace Logic.Scripts.GameDomain.MVC.Skills {
    class AbilityExecutor : MonoBehaviour {
        AbilityData _abilityData;
        GameObject _target;

        public AbilityExecutor(AbilityData abilityData, GameObject target) {
            _abilityData = abilityData;
            _target = target;
        }

    }
}