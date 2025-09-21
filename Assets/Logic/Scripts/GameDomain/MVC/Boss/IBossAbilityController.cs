using UnityEngine;

namespace Logic.Scripts.GameDomain.MVC.Boss
{
    public interface IBossAbilityController
    {
        void CreateAbility(Transform referenceTransform);
        void CreateAbilityAtIndex(int index, Transform referenceTransform);
    }
}


