using System;
using UnityEngine;
using DG.Tweening;
using Logic.Scripts.GameDomain.MVC.Abilitys;
using Logic.Scripts.GameDomain.MVC.Nara;
using Logic.Scripts.GameDomain.MVC.Boss;

namespace Logic.Scripts.GameDomain.MVC.Abilitys.Effects
{
    [Serializable]
    public sealed class KnockbackEffect : AbilityEffect
    {
        [Min(0f)] [SerializeField] private float _force = 2f;
        [Min(0f)] [SerializeField] private float _speed = 6f;

        public override void Execute(IEffectable caster, IEffectable target)
        {
            if (_force <= 0f || target == null) return;
            if (!TryGetNaraRigidbody(target, out var rb)) return;

            Vector3 casterPos = ResolveCasterPosition(caster);

            Vector3 dir = rb.position - casterPos;
            dir.y = 0f;
            if (dir.sqrMagnitude < 1e-6f) return;
            dir.Normalize();

            Vector3 start = rb.position;
            Vector3 end   = start + dir * _force;
            end.y = start.y;

            if (_speed <= 0f)
            {
                rb.MovePosition(end);
                return;
            }

            float duration = _force / _speed;

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
