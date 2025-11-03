using System;
using UnityEngine;

namespace Logic.Scripts.GameDomain.MVC.Abilitys {
    [Serializable]
    public abstract class AbilityEffect {
        public string Name;
        public string Description;
        public bool IsAutoCast;
        protected AbilityData Data;
        public virtual void SetUp(Vector3 point) { }
        public virtual void Execute(AbilityData data, IEffectable caster, IEffectable target) { }
        public virtual void Execute(AbilityData data, IEffectable caster) { }
        public virtual void Execute(IEffectable caster, IEffectable target) { }
        public virtual void Cancel(IEffectable caster, IEffectable target) { }
    }
}