using System.Threading;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Logic.Scripts.Utils;
using UnityEngine;
using UnityEngine.UIElements;

namespace Logic.Scripts.Extensions {
    public static class DOTweenExtensions {
        public static async Awaitable WithCancellationSafe(this Tween tween, CancellationToken cancellationToken) {
            KillTweenImmediatelyWhenTokenIsCanceled(tween, cancellationToken);
            await WaitUntilCompleted(tween, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
        }

        private static async Awaitable WaitUntilCompleted(this Tween tween, CancellationToken cancellationToken) {
            await AwaitableUtils.WaitUntil(() => !tween.active || tween.IsComplete(), cancellationToken);
        }

        public static TweenerCore<float, float, FloatOptions> DOValue(this LoadingScreenBinding target, float endValue, float duration, bool snapping = false) {
            TweenerCore<float, float, FloatOptions> t = DOTween.To(
                () => target.PercentLoaded.value,
                x => target.PercentLoaded = new Length(x, target.PercentLoaded.unit),
                endValue, duration);

            t.SetOptions(snapping).SetTarget(target);
            return t;
        }

        private static void KillTweenImmediatelyWhenTokenIsCanceled(this Tween tween, CancellationToken cancellationToken) {
            cancellationToken.Register(() => {
                if (tween != null && tween.IsActive()) {
                    tween.Kill();
                }
            });
        }
    }
}
