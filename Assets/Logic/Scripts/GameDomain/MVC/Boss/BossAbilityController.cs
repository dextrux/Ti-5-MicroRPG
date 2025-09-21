using UnityEngine;
using Logic.Scripts.GameDomain.MVC.Abilitys;

namespace Logic.Scripts.GameDomain.MVC.Boss
{
    public class BossAbilityController : IBossAbilityController
    {
        private readonly AbilityView[] _abilityPrefabs;
        private int _activeIndex;

        public BossAbilityController(AbilityView[] abilityPrefabs)
        {
            _abilityPrefabs = abilityPrefabs;
            _activeIndex = 0;
        }

        public void CreateAbility(Transform referenceTransform)
        {
            if (_abilityPrefabs == null || _abilityPrefabs.Length == 0) return;
            int index = _activeIndex % _abilityPrefabs.Length;
            AbilityView abilitySpawned = Object.Instantiate(_abilityPrefabs[index], referenceTransform.position, referenceTransform.rotation);
            _activeIndex++;
        }

        public void CreateAbilityAtIndex(int index, Transform referenceTransform)
        {
            if (_abilityPrefabs == null || _abilityPrefabs.Length == 0) return;
            if (index < 0) index = 0;
            index = index % _abilityPrefabs.Length;
            AbilityView abilitySpawned = Object.Instantiate(_abilityPrefabs[index], referenceTransform.position, referenceTransform.rotation);
        }
    }
}


