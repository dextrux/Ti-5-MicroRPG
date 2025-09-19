using UnityEngine;

namespace Logic.Scripts.GameDomain.MVC.Skills {
    class AbilityExecutor : MonoBehaviour {
        AbilityData _abilityData;
        GameObject _target;

        public AbilityExecutor(AbilityData abilityData, GameObject target) {
            _abilityData = abilityData;
            _target = target;
        }

        public void ExecuteAll(GameObject caster, GameObject target) {
            if (_abilityData == null || _abilityData.effects == null) return;
            for (int i = 0; i < _abilityData.effects.Count; i++) {
                IAbilityEffect effect = _abilityData.effects[i];
                effect?.Execute(caster, target);
            }
        }

    }
}