using UnityEngine;

namespace Logic.Scripts.GameDomain.MVC.Skills {
    class AbilityExecutor {
        AbilityData _abilityData;
        GameObject _target;
        GameObject _caster;

        public AbilityExecutor(AbilityData abilityData, GameObject target, GameObject caster) {
            _abilityData = abilityData;
            _target = target;
            _caster = caster;
        }

        public void ExecuteAll(GameObject caster, GameObject target) {
            foreach (AbilityEffect effect in _abilityData.effects) {
                effect.Execute(_caster, target);
            }
        }

    }
}