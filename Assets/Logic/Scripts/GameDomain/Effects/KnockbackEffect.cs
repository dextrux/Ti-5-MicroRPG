using System;
using UnityEngine;
using DG.Tweening;
using Logic.Scripts.GameDomain.MVC.Abilitys;
using Logic.Scripts.GameDomain.MVC.Nara;
using Logic.Scripts.GameDomain.MVC.Boss;

namespace Logic.Scripts.GameDomain.Effects
{
    [Serializable]
    public sealed class KnockbackEffect : AbilityEffect, IForceScaledEffect, IAsyncEffect
    {
        [Min(0f)] [SerializeField] private float _force = 2f;
        [Min(0f)] [SerializeField] private float _speed = 6f;
        private int _stacksMul;
        private int _distanceMul;

        public override void Execute(IEffectable caster, IEffectable target)
        {
            if (target == null) return;
            if (!TryGetNaraRigidbody(target, out var rb)) return;

            Vector3 dir;
            if (Logic.Scripts.GameDomain.MVC.Boss.Attacks.Feather.FeatherLinesHandler.CurrentSpecialAxis != Vector3.zero)
            {
                Vector3 axis = Logic.Scripts.GameDomain.MVC.Boss.Attacks.Feather.FeatherLinesHandler.CurrentSpecialAxis;
                axis.y = 0f; axis.Normalize();
                Vector3 normal = new Vector3(-axis.z, 0f, axis.x).normalized;
                Vector3 player = rb.position;
                Vector3 a = Logic.Scripts.GameDomain.MVC.Boss.Attacks.Feather.FeatherLinesHandler.CurrentSpecialStart;
                Vector3 b = Logic.Scripts.GameDomain.MVC.Boss.Attacks.Feather.FeatherLinesHandler.CurrentSpecialEnd;
                Vector3 ab = (b - a); ab.y = 0f;
                float t = Mathf.Clamp01(Vector3.Dot(player - a, ab) / Mathf.Max(1e-6f, ab.sqrMagnitude));
                Vector3 closest = a + ab * t;
                Vector3 toPlayer = (player - closest);
                float side = Mathf.Sign(Vector3.Dot(normal, toPlayer));
                dir = side * normal;
            }
            else
            {
                Vector3 casterPos = ResolveCasterPosition(caster);
                dir = rb.position - casterPos;
                dir.y = 0f;
                if (dir.sqrMagnitude < 1e-6f) return;
                dir.Normalize();
            }

            // Scalers: stacks 0..5 -> 1x..2x; distance 0..max -> 0.5x..1.5x
            float stacksFactor = 1f + Mathf.Clamp(_stacksMul, 0, 5) * 0.2f;
            // distance multiplier comes as an integer approx in meters; clamp 0..60 (cap)
            float maxMeters = 60f;
            float dMeters = Mathf.Clamp(_distanceMul, 0, maxMeters);
            // far -> 0.5x, near -> 3.0x (range 0.5..3.0)
            float distanceFactor = 0.5f + 2.5f * (1f - (dMeters / maxMeters));
            distanceFactor = Mathf.Clamp(distanceFactor, 0.5f, 3.0f);
            float scaledForce = Mathf.Max(0f, _force * stacksFactor * distanceFactor);
            Debug.Log($"KnockbackEffect: base={_force:0.###} stacks={_stacksMul} stacksFactor={stacksFactor:0.00} distMeters={dMeters:0.###}/{maxMeters:0.###} distanceFactor={distanceFactor:0.00} scaledForce={scaledForce:0.###}");
            if (scaledForce <= 0f) return;

            Vector3 start = rb.position;
            Vector3 end   = start + dir * scaledForce;
            end.y = start.y;

            float duration = 0.45f; // a little less than the handler's 0.5s wait
            DOTween.Kill(rb, complete: false);
            DOVirtual.Float(0f, 1f, duration, v =>
            {
                Vector3 p = Vector3.Lerp(start, end, v);
                p.y = start.y;
                rb.MovePosition(p);
            })
            .SetEase(Ease.Linear)
            .SetUpdate(UpdateType.Fixed)
            .SetId(rb);
        }

        public System.Collections.IEnumerator ExecuteRoutine(IEffectable caster, IEffectable target)
        {
            if (target == null) yield break;
            if (!TryGetNaraRigidbody(target, out var rb)) yield break;
            // Duplicate calculation of dir and scaledForce to run synchronously but yield duration
            Vector3 dir;
            if (Logic.Scripts.GameDomain.MVC.Boss.Attacks.Feather.FeatherLinesHandler.CurrentSpecialAxis != Vector3.zero)
            {
                Vector3 axis = Logic.Scripts.GameDomain.MVC.Boss.Attacks.Feather.FeatherLinesHandler.CurrentSpecialAxis;
                axis.y = 0f; axis.Normalize();
                Vector3 normal = new Vector3(-axis.z, 0f, axis.x).normalized;
                Vector3 player = rb.position;
                Vector3 a = Logic.Scripts.GameDomain.MVC.Boss.Attacks.Feather.FeatherLinesHandler.CurrentSpecialStart;
                Vector3 b = Logic.Scripts.GameDomain.MVC.Boss.Attacks.Feather.FeatherLinesHandler.CurrentSpecialEnd;
                Vector3 ab = (b - a); ab.y = 0f;
                float t = Mathf.Clamp01(Vector3.Dot(player - a, ab) / Mathf.Max(1e-6f, ab.sqrMagnitude));
                Vector3 closest = a + ab * t;
                Vector3 toPlayer = (player - closest);
                float side = Mathf.Sign(Vector3.Dot(normal, toPlayer));
                dir = side * normal;
            }
            else
            {
                Vector3 casterPos = ResolveCasterPosition(caster);
                dir = rb.position - casterPos;
                dir.y = 0f;
                if (dir.sqrMagnitude < 1e-6f) yield break;
                dir.Normalize();
            }
            float stacksFactor = 1f + Mathf.Clamp(_stacksMul, 0, 5) * 0.2f;
            float maxMeters = 60f;
            float dMeters = Mathf.Clamp(_distanceMul, 0, maxMeters);
            float distanceFactor = 0.5f + 2.5f * (1f - (dMeters / maxMeters));
            distanceFactor = Mathf.Clamp(distanceFactor, 0.5f, 3.0f);
            float scaledForce = Mathf.Max(0f, _force * stacksFactor * distanceFactor);
            Vector3 start = rb.position;
            Vector3 end = start + dir * scaledForce;
            end.y = start.y;
            float duration = 0.45f;
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.fixedDeltaTime;
                float k = Mathf.Clamp01(elapsed / duration);
                Vector3 p = Vector3.Lerp(start, end, k);
                rb.MovePosition(new Vector3(p.x, start.y, p.z));
                yield return new WaitForFixedUpdate();
            }
        }

        public void SetForceScalers(int stacksMultiplier, int distanceMultiplier)
        {
            _stacksMul = stacksMultiplier;
            _distanceMul = distanceMultiplier;
        }

        private static bool TryGetNaraRigidbody(IEffectable target, out Rigidbody rb)
        {
            rb = null;
            if (target is NaraController naraController)
            {
                var go = naraController.NaraViewGO;
                if (go != null)
                {
                    var view = go.GetComponent<NaraView>();
                    if (view != null) rb = view.GetRigidbody();
                }
            }
            if (rb == null)
            {
                var view = UnityEngine.Object.FindObjectOfType<NaraView>();
                if (view != null) rb = view.GetRigidbody();
            }
            return rb != null;
        }

        private static Vector3 ResolveCasterPosition(IEffectable caster)
        {
            var bossView = UnityEngine.Object.FindObjectOfType<BossView>();
            if (bossView != null) return bossView.transform.position;

            var naraView = UnityEngine.Object.FindObjectOfType<NaraView>();
            if (naraView != null) return naraView.transform.position;

            return Vector3.zero;
        }
    }
}


