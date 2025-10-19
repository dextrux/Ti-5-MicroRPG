using System;
using UnityEngine;
using DG.Tweening;
using Logic.Scripts.GameDomain.MVC.Abilitys;
using Logic.Scripts.GameDomain.MVC.Nara;
using Logic.Scripts.GameDomain.MVC.Boss;

namespace Logic.Scripts.GameDomain.MVC.Abilitys.Effects
{
    [Serializable]
    public sealed class GrappleEffect : AbilityEffect
    {
        [Min(0f)] [SerializeField] private float _force = 2f;
        [Min(0f)] [SerializeField] private float _stopDistance = 1f;
        [Min(0f)] [SerializeField] private float _speed = 6f;

        public override void Execute(IEffectable caster, IEffectable target)
        {
            if (_force <= 0f || target == null) return;
            if (!TryGetNaraRigidbody(target, out var rb)) return;

            Vector3 casterPos = ResolveCasterPosition(caster);

            Vector3 toCaster = casterPos - rb.position;
            toCaster.y = 0f;

            float dist = toCaster.magnitude;
            if (dist <= _stopDistance + 1e-5f) return;

            float step = Mathf.Min(_force, Mathf.Max(0f, dist - _stopDistance));
            if (step <= 1e-6f) return;

            Vector3 dir = toCaster / dist;

            Vector3 start = rb.position;
            Vector3 end   = start + dir * step;
            end.y = start.y;

            if (_speed <= 0f)
            {
                rb.MovePosition(end);
                return;
            }

            float duration = step / _speed;

            DOTween.Kill(rb, complete: false);

            float t = 0f;
            DOVirtual.Float(0f, 1f, duration, v =>
            {
                t = v;
                Vector3 p = Vector3.Lerp(start, end, t);
                p.y = start.y;
                rb.MovePosition(p);
            })
            .SetEase(Ease.Linear)
            .SetUpdate(UpdateType.Fixed)
            .SetId(rb);
        }

        private static bool TryGetNaraRigidbody(IEffectable target, out Rigidbody rb)
        {
            rb = null;
            if (target is NaraController naraCtrl)
            {
                var go = naraCtrl.NaraViewGO;
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
