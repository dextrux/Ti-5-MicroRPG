using UnityEngine;
using System.Collections.Generic;
using Logic.Scripts.GameDomain.MVC.Abilitys;
using Logic.Scripts.GameDomain.MVC.Boss.Attacks.Core;
using Logic.Scripts.GameDomain.MVC.Boss.Attacks.Cone;
using Logic.Scripts.GameDomain.MVC.Boss.Attacks.Feather;
using Logic.Scripts.GameDomain.MVC.Boss.Attacks.Orb;
using Logic.Scripts.GameDomain.Commands;
using Logic.Scripts.Services.CommandFactory;
using Zenject;

namespace Logic.Scripts.GameDomain.MVC.Boss
{
    public class BossAttack : MonoBehaviour
    {
        [SerializeReference] private List<AbilityEffect> _effects;

        private enum AttackType { ProteanCones, FeatherLines, Orb, HookAwakening }
        [SerializeField] private AttackType _attackType = AttackType.ProteanCones;

        [SerializeField] private ProteanConesParams _protean = new ProteanConesParams { radius = 3f, angleDeg = 60f, sides = 36 };

        [SerializeField] private FeatherLinesParams _feather = new FeatherLinesParams { featherCount = 3, axisMode = FeatherAxisMode.XZ, width = 2f, margin = 5f, forceBase = 2f, forcePerMeter = 0.4f, forcePerDebuff = 0.5f };

        [System.Serializable]
        private struct OrbSpawnParams
        {
            public GameObject prefab;
            public float moveStepMeters;
            public float growStepMeters;
            public float initialRadius;
            public float maxRadiusCap;
            public int baseDamage;
            public int initialHp;
        }

        [SerializeField] private OrbSpawnParams _orb = new OrbSpawnParams { prefab = null, moveStepMeters = 6f, growStepMeters = 6f, initialRadius = 4f, maxRadiusCap = 60f, baseDamage = 10, initialHp = 50 };

        private ArenaPosReference _arena;
        private IEffectable _caster;
        private IBossAttackHandler _handler;
        private bool _executing;
        private System.Threading.Tasks.TaskCompletionSource<bool> _executeTcs;
        private ICommandFactory _commandFactory;

        public void Setup(ArenaPosReference arena, IEffectable caster)
        {
            _arena = arena;
            _caster = caster;
            try { _commandFactory = ProjectContext.Instance.Container.Resolve<ICommandFactory>(); } catch { _commandFactory = null; }
            SelectAndBuildHandler();
            Transform parentForTelegraph = transform;
            if (_attackType == AttackType.FeatherLines)
            {
                parentForTelegraph = _arena != null ? _arena.transform : transform;
            }
            _handler?.PrepareTelegraph(parentForTelegraph);
        }

        public void Execute()
        {
            if (_attackType == AttackType.Orb)
            {
                if (_executeTcs == null) _executeTcs = new System.Threading.Tasks.TaskCompletionSource<bool>();
                if (_executing) return;
                _executing = true;
                TrySpawnOrb();
                CleanupAndComplete();
                return;
            }
            if (_handler == null) { Destroy(gameObject); return; }
            if (_executeTcs == null) _executeTcs = new System.Threading.Tasks.TaskCompletionSource<bool>();
            if (_executing) return;
            _executing = true;
            bool hit = _handler.ComputeHits(_arena, transform, _caster);
            StartCoroutine(ExecuteAndCleanup());
        }

        public System.Threading.Tasks.Task ExecuteAsync()
        {
            if (_attackType == AttackType.Orb)
            {
                if (_executeTcs == null) _executeTcs = new System.Threading.Tasks.TaskCompletionSource<bool>();
                if (!_executing)
                {
                    _executing = true;
                    TrySpawnOrb();
                    CleanupAndComplete();
                }
                return _executeTcs.Task;
            }
            if (_handler == null) { return System.Threading.Tasks.Task.CompletedTask; }
            if (_executeTcs == null) _executeTcs = new System.Threading.Tasks.TaskCompletionSource<bool>();
            if (!_executing)
            {
                _executing = true;
                bool hit = _handler.ComputeHits(_arena, transform, _caster);
                StartCoroutine(ExecuteAndCleanup());
            }
            return _executeTcs.Task;
        }

        private System.Collections.IEnumerator ExecuteAndCleanup()
        {
            if (_effects != null)
            {
                yield return _handler.ExecuteEffects(_effects, _arena, transform, _caster);
            }
            CleanupAndComplete();
        }

        private void CleanupAndComplete()
        {
            _handler?.Cleanup();
            Destroy(gameObject);
            if (_executeTcs != null && !_executeTcs.Task.IsCompleted) _executeTcs.TrySetResult(true);
        }

        private void TrySpawnOrb()
        {
            if (_orb.prefab == null)
            {
                Debug.LogWarning("BossAttack Orb: prefab is null");
                return;
            }
            if (_commandFactory != null)
            {
                var spawnByFactory = _commandFactory.CreateCommandVoid<SpawnOrbCommand>();
                // Try resolve registry from the same subscene container
                Logic.Scripts.GameDomain.MVC.Environment.Orb.OrbRegistry reg = null;
                try { reg = ProjectContext.Instance.Container.Resolve<Logic.Scripts.GameDomain.MVC.Environment.Orb.OrbRegistry>(); } catch { reg = null; }
                spawnByFactory.SetData(new SpawnOrbData
                {
                    Arena = _arena,
                    Origin = transform.position,
                    Prefab = _orb.prefab,
                    Registry = reg,
                    MoveStep = _orb.moveStepMeters,
                    GrowStep = _orb.growStepMeters,
                    InitialRadius = _orb.initialRadius,
                    MaxRadius = _orb.maxRadiusCap,
                    BaseDamage = _orb.baseDamage,
                    InitialHp = _orb.initialHp
                });
                UnityEngine.Debug.Log($"[BossAttack][Orb] Spawning via CommandFactory at {transform.position}");
                spawnByFactory.Execute();
                return;
            }
            // Fallback without DI (registry won’t be resolved)
            var spawn = new SpawnOrbCommand();
            spawn.ResolveDependencies();
            spawn.SetData(new SpawnOrbData
            {
                Arena = _arena,
                Origin = transform.position,
                Prefab = _orb.prefab,
                MoveStep = _orb.moveStepMeters,
                GrowStep = _orb.growStepMeters,
                InitialRadius = _orb.initialRadius,
                MaxRadius = _orb.maxRadiusCap,
                BaseDamage = _orb.baseDamage,
                InitialHp = _orb.initialHp
            });
            UnityEngine.Debug.Log($"[BossAttack][Orb] Spawning via fallback at {transform.position}");
            spawn.Execute();
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
                case AttackType.Orb:
                {
                    _handler = new OrbSpawnHandler(_orb.initialRadius);
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


