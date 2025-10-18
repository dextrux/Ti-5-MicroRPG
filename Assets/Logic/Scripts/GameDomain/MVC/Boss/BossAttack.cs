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

        [SerializeField] private FeatherLinesParams _feather = new FeatherLinesParams { featherCount = 3, axisMode = FeatherAxisMode.XZ, width = 2f, margin = 5f };

        private ArenaPosReference _arena;
        private IEffectable _caster;
        private IBossAttackHandler _handler;

        public void Setup(ArenaPosReference arena, IEffectable caster)
        {
            _arena = arena;
            _caster = caster;
            SelectAndBuildHandler();
            _handler?.PrepareTelegraph(transform);
        }

        public void Execute()
        {
            if (_handler == null) { Destroy(gameObject); return; }
            bool hit = _handler.ComputeHits(_arena, transform, _caster);
            if (hit && _effects != null)
            {
                IEffectable target = _arena.NaraController as IEffectable;
                if (target == null) { _handler.Cleanup(); Destroy(gameObject); return; }
                for (int i = 0; i < _effects.Count; i++)
                {
                    AbilityEffect fx = _effects[i];
                    if (fx != null) fx.Execute(_caster, target);
                }
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


