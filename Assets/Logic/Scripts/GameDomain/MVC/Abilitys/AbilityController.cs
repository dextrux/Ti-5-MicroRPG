using Logic.Scripts.GameDomain.MVC.Ui;
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
        public AbilityView[] ActiveAbilities => _activeSet;
        int Index;

        public AbilityController(ICommandFactory commandFactory, AbilityView[] abilitieSet1, AbilityView[] abilitieSet2, AbilityView[] abilitieSet3) {
            _commandFactory = commandFactory;
            _abilitySet1 = abilitieSet1;
            _abilitySet2 = abilitieSet2;
            _abilitySet3 = abilitieSet3;
            _activeSet = abilitieSet1;
            Index = 1;
            UpdateUi();
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

        public void UpdateUi() {
            GamePlayUiView.Instance.Skill1Cost = _activeSet[0].AbilityData.Cost;
            GamePlayUiView.Instance.Skill1Name = _activeSet[0].AbilityData.Name;

            GamePlayUiView.Instance.Skill2Cost = _activeSet[0].AbilityData.Cost;
            GamePlayUiView.Instance.Skill2Name = _activeSet[0].AbilityData.Name;
        }

        public void CreateAbility(Transform referenceTransform, int abilitySlotIndex) {
            ShapeSpawner.Instance.Spawn(_activeSet[abilitySlotIndex].gameObject, referenceTransform, _activeSet[abilitySlotIndex].AbilityData.TypeShape);
        }

        public int FindIndexAbility(AbilityView abilityViewToSearch) {
            int aux = -1;
            for (int i = 0; i < _activeSet.Count(); i++) {
                if (_activeSet[i].name == abilityViewToSearch.name) {
                    return i;
                }
            }
            return aux;
        }
    }
}