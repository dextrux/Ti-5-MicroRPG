using UnityEngine;

namespace Logic.Scripts.GameDomain.MVC.Abilitys {
    public interface IAbilityEffect {
        //To-do Substituir pelos devidos Casters
        void Execute(IEffectable caster, IEffectable target);
    }
}