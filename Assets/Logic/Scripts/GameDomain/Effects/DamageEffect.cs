using Logic.Scripts.GameDomain.MVC.Abilitys;
using System;

namespace Assets.Logic.Scripts.GameDomain.Effects {
    [Serializable]
    public class DamageEffect : AbilityEffect {
        public int amount;

        public override void Execute(IEffectable caster, IEffectable target) {
            if (caster == target) return;
            target.TakeDamage(amount);
        }

        public override void Execute(AbilityData data, IEffectable caster, IEffectable target) {
            if (caster == target) return;
            target.TakeDamage(amount + data.GetDamage());
        }
    }
}