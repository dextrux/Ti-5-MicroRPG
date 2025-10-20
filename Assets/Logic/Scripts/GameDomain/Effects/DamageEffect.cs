using Logic.Scripts.GameDomain.MVC.Abilitys;
using System;
using UnityEngine;

namespace Assets.Logic.Scripts.GameDomain.Effects {
    [Serializable]
    public class DamageEffect : AbilityEffect {
        public int amount;

        public override void Execute(IEffectable caster, IEffectable target) {
            Debug.Log("Caster: " + caster.ToString());
            Debug.Log("Target: " + target.ToString());
            target.TakeDamage(amount);
            Debug.Log("Dano sofrido: " + amount);
        }

        public override void Execute(AbilityData data, IEffectable caster, IEffectable target) {
            Debug.Log("Caster: " + caster.ToString());
            Debug.Log("Target: " + target.ToString());
            target.TakeDamage(amount + data.GetDamage());
            Debug.Log("Dano sofrido: " + amount);
        }
    }
}