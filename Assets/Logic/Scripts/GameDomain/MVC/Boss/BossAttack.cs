using UnityEngine;
using System.Collections.Generic;
using Logic.Scripts.GameDomain.MVC.Abilitys;
using Logic.Scripts.GameDomain.MVC.Boss.Attacks.Core;
using Logic.Scripts.GameDomain.MVC.Boss.Attacks.Cone;
using Logic.Scripts.GameDomain.MVC.Boss.Attacks.Feather;

namespace Logic.Scripts.GameDomain.MVC.Boss
{
    public class BossAttack : MonoBehaviour
    {
        [SerializeReference] private List<AbilityEffect> _effects;

        private enum AttackType { ProteanCones, FeatherLines, Orb, HookAwakening }
        [SerializeField] private AttackType _attackType = AttackType.ProteanCones;

        [SerializeField] private ProteanConesParams _protean = new ProteanConesParams { radius = 3f, angleDeg = 60f, sides = 36 };

        [SerializeField] private FeatherLinesParams _feather = new FeatherLinesParams { featherCount = 3, axisMode = FeatherAxisMode.XZ, width = 2f, margin = 5f, forceBase = 2f, forcePerMeter = 0.4f, forcePerDebuff = 0.5f };

        private ArenaPosReference _arena;
        private IEffectable _caster;
        private IBossAttackHandler _handler;

        public void Setup(ArenaPosReference arena, IEffectable caster)
        {
            _arena = arena;
            _caster = caster;
            SelectAndBuildHandler();
            _handler?.PrepareTelegraph(_arena != null ? _arena.transform : transform);
        }

        public void Execute()
        {
            if (_handler == null) { Destroy(gameObject); return; }
            bool hit = _handler.ComputeHits(_arena, transform, _caster);
            StartCoroutine(ExecuteAndCleanup());
        }

        private System.Collections.IEnumerator ExecuteAndCleanup()
        {
            if (_effects != null)
            {
                yield return _handler.ExecuteEffects(_effects, _arena, transform, _caster);
            }
            _handler.Cleanup();
            Destroy(gameObject);
        }

        private void SelectAndBuildHandler()
        {
            switch (_attackType)
            {
                case AttackType.ProteanCones:
                {
                    float[] yaws = new float[] { 0f, 90f, 180f, 270f };
                    _handler = new ConeAttackHandler(_protean.radius, _protean.angleDeg, _protean.sides, yaws);
                    break;
                }
                case AttackType.FeatherLines:
                {
                    _handler = new FeatherLinesHandler(_feather);
                    break;
                }
                default:
                {
                    _handler = null;
                    break;
                }
            }
        }
    }
}


