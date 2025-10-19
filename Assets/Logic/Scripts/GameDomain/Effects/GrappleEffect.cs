using System;
using UnityEngine;
using DG.Tweening;
using Logic.Scripts.GameDomain.MVC.Abilitys;
using Logic.Scripts.GameDomain.MVC.Nara;
using Logic.Scripts.GameDomain.MVC.Boss;

namespace Logic.Scripts.GameDomain.Effects
{
    [Serializable]
    public sealed class GrappleEffect : AbilityEffect, IForceScaledEffect, IAsyncEffect
    {
        [Min(0f)] [SerializeField] private float _force = 2f;
        [Min(0f)] [SerializeField] private float _stopDistance = 1f;
        [Min(0f)] [SerializeField] private float _speed = 6f;
        [SerializeField] private static int _stacksMul = 0;
        private int _distanceMul;

        public override void Execute(IEffectable caster, IEffectable target)
        {
            _stacksMul += 1;
            Debug.Log($"Grapple foi, Stack = {_stacksMul}");
            if (target == null) return;
            if (!TryGetNaraRigidbody(target, out var rb)) return;

            Vector3 axis = Logic.Scripts.GameDomain.MVC.Boss.Attacks.Feather.FeatherLinesHandler.CurrentSpecialAxis;
            Vector3 a = Logic.Scripts.GameDomain.MVC.Boss.Attacks.Feather.FeatherLinesHandler.CurrentSpecialStart;
            Vector3 b = Logic.Scripts.GameDomain.MVC.Boss.Attacks.Feather.FeatherLinesHandler.CurrentSpecialEnd;
            axis.y = 0f; axis.Normalize();
            Vector3 normal = new Vector3(-axis.z, 0f, axis.x).normalized;
            Vector3 player = rb.position;
            Vector3 ab = (b - a); ab.y = 0f;
            float t = Mathf.Clamp01(Vector3.Dot(player - a, ab) / Mathf.Max(1e-6f, ab.sqrMagnitude));
            Vector3 closest = a + ab * t;
            Vector3 toLine = closest - player;
            toLine.y = 0f;
            float dist = toLine.magnitude;
            if (dist <= _stopDistance + 1e-5f) return;

            // Scalers: stacks 0..5 -> 1x..2x; distance 0..max -> 0.5x..1.5x
            float stacksFactor = 1f + Mathf.Clamp(_stacksMul, 0, 5) * 0.2f;
            float maxMeters = 60f;
            float dMeters = Mathf.Clamp(_distanceMul, 0, maxMeters);
            float distanceFactor = 0.5f + 2.5f * (1f - (dMeters / maxMeters));
            distanceFactor = Mathf.Clamp(distanceFactor, 0.5f, 3.0f);
            float maxForce = Mathf.Max(0f, _force * stacksFactor * distanceFactor);
            float step = Mathf.Min(maxForce, Mathf.Max(0f, dist - _stopDistance));
            Debug.Log($"GrappleEffect: base={_force:0.###} stacks={_stacksMul} stacksFactor={stacksFactor:0.00} distMeters={dMeters:0.###}/{maxMeters:0.###} distanceFactor={distanceFactor:0.00} maxForce={maxForce:0.###} step={step:0.###}");
            if (step <= 1e-6f) return;

            Vector3 dir = toLine / dist;
            Vector3 start = rb.position;
            Vector3 end = start + dir * step;
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
            Vector3 axis = Logic.Scripts.GameDomain.MVC.Boss.Attacks.Feather.FeatherLinesHandler.CurrentSpecialAxis;
            Vector3 a = Logic.Scripts.GameDomain.MVC.Boss.Attacks.Feather.FeatherLinesHandler.CurrentSpecialStart;
            Vector3 b = Logic.Scripts.GameDomain.MVC.Boss.Attacks.Feather.FeatherLinesHandler.CurrentSpecialEnd;
            axis.y = 0f; axis.Normalize();
            Vector3 player = rb.position;
            Vector3 ab = (b - a); ab.y = 0f;
            float t = Mathf.Clamp01(Vector3.Dot(player - a, ab) / Mathf.Max(1e-6f, ab.sqrMagnitude));
            Vector3 closest = a + ab * t;
            Vector3 toLine = closest - player;
            toLine.y = 0f;
            float dist = toLine.magnitude;
            float stacksFactor = 1f + Mathf.Clamp(_stacksMul, 0, 5) * 0.2f;
            float maxMeters = 60f;
            float dMeters = Mathf.Clamp(_distanceMul, 0, maxMeters);
            float distanceFactor = 0.5f + 2.5f * (1f - (dMeters / maxMeters));
            distanceFactor = Mathf.Clamp(distanceFactor, 0.5f, 3.0f);
            float maxForce = Mathf.Max(0f, _force * stacksFactor * distanceFactor);
            float step = Mathf.Min(maxForce, Mathf.Max(0f, dist - _stopDistance));
            if (step <= 1e-6f) yield break;
            Vector3 dir = toLine / Mathf.Max(1e-6f, dist);
            Vector3 start = rb.position;
            Vector3 end = start + dir * step;
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

            Debug.Log($"Grapple foi, Stack = {_stacksMul}");
        }

        public void SetForceScalers(int stacksMultiplier, int distanceMultiplier)
        {
            //_stacksMul = stacksMultiplier;
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


