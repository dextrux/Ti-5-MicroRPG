using System;
using UnityEngine;
using Logic.Scripts.GameDomain.MVC.Common;

namespace Logic.Scripts.GameDomain.MVC.Skills {
    [Serializable]
    public class AbilityEffect : IAbilityEffect {
        public void Execute(GameObject caster, GameObject target) {
            // placeholder de dano simples
            var damageable = target.GetComponent<IDamageable>();
            if (damageable != null) {
                damageable.TakeDamage(10);
            }
        }
    }
}