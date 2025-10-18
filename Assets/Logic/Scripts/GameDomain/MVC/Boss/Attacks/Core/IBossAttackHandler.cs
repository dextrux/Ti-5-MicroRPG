using UnityEngine;

namespace Logic.Scripts.GameDomain.MVC.Boss.Attacks.Core
{
    public interface IBossAttackHandler
    {
        void PrepareTelegraph(Transform parentTransform);
        bool ComputeHits(ArenaPosReference arenaReference, Transform originTransform, IEffectable caster);
        void Cleanup();
    }
}


