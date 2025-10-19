using System.Collections.Generic;
using UnityEngine;
using Logic.Scripts.GameDomain.MVC.Abilitys;

namespace Logic.Scripts.GameDomain.MVC.Boss.Attacks.Core
{
    public interface IBossAttackHandler
    {
        void PrepareTelegraph(Transform parentTransform);
        bool ComputeHits(ArenaPosReference arenaReference, Transform originTransform, IEffectable caster);
        System.Collections.IEnumerator ExecuteEffects(List<AbilityEffect> effects, ArenaPosReference arenaReference, Transform originTransform, IEffectable caster);
        void Cleanup();
    }
}


