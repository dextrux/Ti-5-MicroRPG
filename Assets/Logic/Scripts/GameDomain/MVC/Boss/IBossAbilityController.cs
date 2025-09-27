using UnityEngine;

namespace Logic.Scripts.GameDomain.MVC.Boss
{
    public interface IBossAbilityController
    {
        BossAttack CreateAttack(Transform referenceTransform);
        BossAttack CreateAttackAtIndex(int index, Transform referenceTransform);
    }
}


