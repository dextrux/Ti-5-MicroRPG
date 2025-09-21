using Logic.Scripts.GameDomain.MVC.Abilitys;
using System;
using UnityEngine;

namespace Assets.Logic.Scripts.GameDomain.Effects {
    [Serializable]
    public class DamageEffect : AbilityEffect {
        public int amount;

        public override void Execute(IEffectable caster, IEffectable target) {
            Debug.Log($"{caster} dealt {amount} damage to {target}");
        }
    }
}