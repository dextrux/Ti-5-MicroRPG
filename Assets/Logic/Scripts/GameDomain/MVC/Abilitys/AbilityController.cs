using Logic.Scripts.GameDomain.MVC.Nara;
using Logic.Scripts.GameDomain.MVC.Ui;
using Logic.Scripts.Services.CommandFactory;
using System.Linq;
using UnityEngine;

namespace Logic.Scripts.GameDomain.MVC.Abilitys {
    public class AbilityController : IAbilityController {
        private readonly ICommandFactory _commandFactory;
        private readonly IGamePlayUiController _gamePlayUiController;

        private readonly AbilityData[] _abilitySet1;
        private readonly AbilityData[] _abilitySet2;
        private readonly AbilityData[] _abilitySet3;
        public Transform PlayerTransform;

        private AbilityData[] _activeSet;
        public AbilityData[] ActiveAbilities => _activeSet;
        int Index;

        public AbilityController(ICommandFactory commandFactory, AbilityData[] abilitieSet1, AbilityData[] abilitieSet2,
            AbilityData[] abilitieSet3, IGamePlayUiController gamePlayUiController) {
            _commandFactory = commandFactory;
            _gamePlayUiController = gamePlayUiController;
            _abilitySet1 = abilitieSet1;
            _abilitySet2 = abilitieSet2;
            _abilitySet3 = abilitieSet3;
            _activeSet = abilitieSet1;
            _activeSet = _abilitySet1;
            Index = 1;
        }

        public void InitEntryPoint(INaraController naraController) {
            UpdateUi();
            PlayerTransform = naraController.NaraViewGO.transform;
        }

        private void UpdateUi() {
            _gamePlayUiController.SetAbilityValues(
                            _activeSet[0].GetCost(), _activeSet[0].Name,
                            _activeSet[1].GetCost(), _activeSet[1].Name,
                            _activeSet[2].GetCost(), _activeSet[2].Name
                            );
        }

        public void NextSet() {
            Index++;
            if (Index >= 4) Index = 1;
            ChangeActiveSet(Index);
        }

        public void PreviousSet() {
            Index--;
            if (Index <= 0) Index = 3;
            ChangeActiveSet(Index);
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
            UpdateUi();
        }
        public void CreateAbility(Transform referenceTransform, int abilitySlotIndex) {
            //To-Do Setar habilidades
        }
        
        public void CreateAbility(Transform referenceTransform, AbilityData abilityToSpawn) {
            //To-Do Setar habilidades
         }

        public int FindIndexAbility(AbilityData abilityToSearch) {
            int aux = -1;
            for (int i = 0; i < _activeSet.Count(); i++) {
                if (_activeSet[i].name == abilityToSearch.name) {
                    return i;
                }
            }
            return aux;
        }
    }
}