using System;
using Logic.Scripts.GameDomain.MVC.Nara;
using UnityEngine;

namespace Logic.Scripts.GameDomain.MVC.Abilitys {
    [Serializable]
    public abstract class AbilityEffect : IAbilityEffect {
        public abstract void Execute(IEffectable caster, IEffectable target);
    }
}