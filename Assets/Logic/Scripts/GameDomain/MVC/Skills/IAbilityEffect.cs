using UnityEngine;

namespace Logic.Scripts.GameDomain.MVC.Skills {
    public interface IAbilityEffect {
        //To-do Substituir pelos devidos Casters
        void Execute(GameObject caster, GameObject target);
    }
}