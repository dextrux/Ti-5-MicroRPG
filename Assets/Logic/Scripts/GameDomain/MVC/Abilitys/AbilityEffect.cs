using System;
using UnityEngine;

namespace Logic.Scripts.GameDomain.MVC.Abilitys {
    [Serializable]
    public abstract class AbilityEffect : IAbilityEffect {
        public void Execute(GameObject caster, GameObject target) {
            
        }
    }
}