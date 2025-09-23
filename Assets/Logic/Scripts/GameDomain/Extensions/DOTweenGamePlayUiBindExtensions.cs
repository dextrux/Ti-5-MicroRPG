using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine.UIElements;

namespace Logic.Scripts.GameDomain.Extensions {
    public static class DOTweenGamePlayUiBindExtensions {
        public static TweenerCore<float, float, FloatOptions> DOValue(
        this GamePlayUiBindSO target,
        System.Func<GamePlayUiBindSO, Length> getter,
        System.Action<GamePlayUiBindSO, Length> setter,
        float endValue,
        float duration,
        bool snapping = false) {
            TweenerCore<float, float, FloatOptions> t = DOTween.To(
                () => getter(target).value,
                x => setter(target, new Length(x, getter(target).unit)),
                endValue,
                duration
            );
            t.SetOptions(snapping).SetTarget(target);
            return t;
        }

    }
}