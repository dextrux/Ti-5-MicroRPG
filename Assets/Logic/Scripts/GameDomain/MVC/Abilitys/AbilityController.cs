using Logic.Scripts.Services.CommandFactory;
using System.Linq;
using UnityEngine;

namespace Logic.Scripts.GameDomain.MVC.Abilitys {
    public class AbilityController : IAbilityController {
        private readonly ICommandFactory _commandFactory;

        private readonly AbilityView[] _abilitySet1;
        private readonly AbilityView[] _abilitySet2;
        private readonly AbilityView[] _abilitySet3;
        private AbilityView[] _activeSet;

        public AbilityController(ICommandFactory commandFactory, AbilityView[] abilitieSet1, AbilityView[] abilitieSet2, AbilityView[] abilitieSet3) {
            _commandFactory = commandFactory;
            _abilitySet1 = abilitieSet1;
            _abilitySet2 = abilitieSet2;
            _abilitySet3 = abilitieSet3;
            _activeSet = abilitieSet1;
            foreach(AbilityView view in _activeSet) {
                Debug.Log(view.name);
            }
            if (_activeSet.Count() == 0) Debug.Log("Zerado irmão");
        }

        public void ChangeActiveSet(int newIndexToActive) {
            switch (newIndexToActive) {
                case 1:
                    _activeSet = _abilitySet1;
                    break;
                case 2:
                    _activeSet = _abilitySet2;
                    break;
                case 3:
                    _activeSet = _abilitySet3;
                    break;
            }
        }

        public void CreateAbility(Transform referenceTransform, int abilitySlotIndex) {
            AbilityView abilitySpawned = null;
            abilitySpawned = Object.Instantiate(_activeSet[abilitySlotIndex], referenceTransform.position, referenceTransform.rotation);
        }
    }
}