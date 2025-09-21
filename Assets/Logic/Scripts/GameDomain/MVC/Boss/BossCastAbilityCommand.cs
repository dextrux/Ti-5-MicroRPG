using Logic.Scripts.Services.CommandFactory;
using Logic.Scripts.GameDomain.MVC.Abilitys;
using UnityEngine;
using Zenject;

namespace Logic.Scripts.GameDomain.MVC.Boss
{
    public class BossCastAbilityCommand : ICommandVoid
    {
        private AbilityData _ability;
        private GameObject _caster;
        private GameObject _target;

        public void SetObjectResolver(DiContainer diContainer) { }
        public void ResolveDependencies() { }

        public void SetContext(AbilityData ability, GameObject caster, GameObject target)
        {
            _ability = ability;
            _caster = caster;
            _target = target;
        }

        public void Execute()
        {
            if (_ability == null || _caster == null) return;
            //var executor = new AbilityExecutor(_ability, _target);
            //executor.ExecuteAll(_caster, _target);
        }
    }
}
