using System;

namespace Logic.Scripts.GameDomain.MVC.Abilitys {
    [Serializable]
    public abstract class AbilityEffect {
        public string Name;
        public string Description;
        protected AbilityData Data;
        public virtual void Execute(AbilityData data, IEffectable caster, IEffectable target) { }
        public virtual void Execute(IEffectable caster, IEffectable target) { }
        public virtual void Cancel(IEffectable caster, IEffectable target) { }
    }
}