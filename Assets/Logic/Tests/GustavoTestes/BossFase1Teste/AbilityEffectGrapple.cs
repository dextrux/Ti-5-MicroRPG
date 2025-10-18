using System;
using UnityEngine;
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

        public override void Execute(IEffectable caster, IEffectable target)
        {
            if (_force <= 0f || target == null) return;

            // 1) Rigidbody da Nara (target)
            if (!TryGetNaraRigidbody(target, out var rb)) return;

            // 2) Posição do boss (caster) como "âncora" do puxão
            Vector3 casterPos = ResolveCasterPosition(caster);

            // 3) Vetor até o boss (plano XZ)
            Vector3 toCaster = casterPos - rb.position;
            toCaster.y = 0f;

            float dist = toCaster.magnitude;
            if (dist <= _stopDistance + 1e-5f) return; // já dentro/igual ao raio de parada

            // 4) Passo até não ultrapassar o stopDistance
            float step = Mathf.Min(_force, Mathf.Max(0f, dist - _stopDistance));
            if (step <= 1e-6f) return;

            Vector3 dir = toCaster / dist; // normalizado (plano)
            Vector3 targetPos = rb.position + dir * step;
            targetPos.y = rb.position.y;

            rb.MovePosition(targetPos);
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
