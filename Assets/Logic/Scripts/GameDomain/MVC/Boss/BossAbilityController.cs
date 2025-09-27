using UnityEngine;
using Logic.Scripts.GameDomain.MVC.Abilitys;

namespace Logic.Scripts.GameDomain.MVC.Boss
{
    public class BossAbilityController : IBossAbilityController
    {
        private readonly BossBehaviorSO _bossBehavior;
        private int _activeIndex;

        public BossAbilityController(BossBehaviorSO bossBehavior)
        {
            _bossBehavior = bossBehavior;
            _activeIndex = 0;
        }

        public BossAttack CreateAttack(Transform referenceTransform)
        {
            BossAttack[] pool = _bossBehavior != null ? _bossBehavior.AvailableAttacks : null;
            if (pool == null || pool.Length == 0) return null;
            int index = _activeIndex % pool.Length;
            BossAttack abilitySpawned = Object.Instantiate(pool[index], referenceTransform.position, referenceTransform.rotation);
            _activeIndex++;
            return abilitySpawned;
        }

        public BossAttack CreateAttackAtIndex(int index, Transform referenceTransform)
        {
            BossAttack[] pool = _bossBehavior != null ? _bossBehavior.AvailableAttacks : null;
            if (pool == null || pool.Length == 0) return null;
            if (index < 0) index = 0;
            index = index % pool.Length;
            BossAttack abilitySpawned = Object.Instantiate(pool[index], referenceTransform.position, referenceTransform.rotation);
            return abilitySpawned;
        }
    }
}


