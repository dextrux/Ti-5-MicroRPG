using UnityEngine;

namespace Logic.Scripts.GameDomain.MVC.Abilitys {
    public interface IAbilityEffect {
        void Execute(IEffectable caster, IEffectable target);
    }
}