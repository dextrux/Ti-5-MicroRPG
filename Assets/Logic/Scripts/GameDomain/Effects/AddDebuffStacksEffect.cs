using System;
using Logic.Scripts.GameDomain.MVC.Abilitys;
using Logic.Scripts.GameDomain.MVC.Nara;

namespace Logic.Scripts.GameDomain.Effects
{
    [Serializable]
    public class AddDebuffStacksEffect : AbilityEffect
    {
        public int amount = 1;

        public override void Execute(IEffectable caster, IEffectable target)
        {
            var nara = target as NaraController;
            if (nara == null) return;
            int add = amount <= 0 ? 0 : amount;
            nara.AddDebuffStacks(add);
        }
    }
}


