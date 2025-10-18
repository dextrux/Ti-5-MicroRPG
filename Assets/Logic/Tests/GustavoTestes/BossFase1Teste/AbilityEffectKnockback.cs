using System;
using UnityEngine;
using Logic.Scripts.GameDomain.MVC.Abilitys;
using Logic.Scripts.GameDomain.MVC.Nara;
using Logic.Scripts.GameDomain.MVC.Boss;

namespace Logic.Scripts.GameDomain.MVC.Abilitys.Effects
{
    [Serializable]
    public sealed class KnockbackEffect : AbilityEffect
    {
        [Min(0f)] [SerializeField] private float _force = 2f;

        public override void Execute(IEffectable caster, IEffectable target)
        {
            if (_force <= 0f || target == null) return;

            // 1) Resolve o Rigidbody da Nara (target)
            if (!TryGetNaraRigidbody(target, out var rb)) return;

            // 2) Resolve a posição do "caster" (boss) para calcular direção radial
            Vector3 casterPos = ResolveCasterPosition(caster);

            // 3) Direção plana (Nara -> longe do boss)
            Vector3 dir = rb.position - casterPos;
            dir.y = 0f;
            if (dir.sqrMagnitude < 1e-6f) return;
            dir.Normalize();

            // 4) Aplica deslocamento via MovePosition (plano XZ)
            Vector3 targetPos = rb.position + dir * _force;
            targetPos.y = rb.position.y;
            rb.MovePosition(targetPos);
        }

        private static bool TryGetNaraRigidbody(IEffectable target, out Rigidbody rb)
        {
            rb = null;
            // Caminho direto: NaraController -> NaraView -> Rigidbody
            if (target is NaraController naraCtrl)
            {
                var go = naraCtrl.NaraViewGO;
                if (go != null)
                {
                    var view = go.GetComponent<NaraView>();
                    if (view != null) rb = view.GetRigidbody();
                }
            }
            // Fallback (caso precise): acha a primeira NaraView na cena
            if (rb == null)
            {
                var view = UnityEngine.Object.FindObjectOfType<NaraView>();
                if (view != null) rb = view.GetRigidbody();
            }
            return rb != null;
        }

        private static Vector3 ResolveCasterPosition(IEffectable caster)
        {
            // Melhor esforço: usa BossView se existir na cena; senão tenta NaraView; por último, (0,0,0)
            var bossView = UnityEngine.Object.FindObjectOfType<BossView>();
            if (bossView != null) return bossView.transform.position;

            var naraView = UnityEngine.Object.FindObjectOfType<NaraView>();
            if (naraView != null) return naraView.transform.position;

            return Vector3.zero;
        }
    }
}
