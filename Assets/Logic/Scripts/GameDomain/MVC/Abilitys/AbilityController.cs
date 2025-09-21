using Logic.Scripts.Services.CommandFactory;
using UnityEngine;

namespace Logic.Scripts.GameDomain.MVC.Abilitys {
    public class AbilityController : IAbilityController {
        private readonly ICommandFactory _commandFactory;

        public AbilityController(ICommandFactory commandFactory) {
            _commandFactory = commandFactory;
        }

        public void CreateAbility(Transform referenceTransform, AbilityView abilityViewPrefab) {
            AbilityView abilitySpawned = null;
            abilitySpawned = Object.Instantiate(abilityViewPrefab, referenceTransform.position, referenceTransform.rotation);
        }
    }
}