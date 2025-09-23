using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;
using Logic.Scripts.Extensions;

namespace Logic.Scripts.Core.Mvc.LoadingScreen {
    public class LoadingScreenView : MonoBehaviour {
        private const int ZERO_INT = 0;

        [SerializeField] private UIDocument _loadingUiDocument;
        [SerializeField] private float _animationDuration = 0.5f;
        [SerializeField] private Ease _animationEase = Ease.OutQuad;

        private VisualElement _fillBar;
        private Tween _currentAnimationTween;
        [SerializeField] private LoadingScreenBinding _loadingBarPercentage;

        public void InitPoint() {
            _loadingBarPercentage.PercentLoaded.value = ZERO_INT;
            _loadingBarPercentage.PercentLoaded.unit = LengthUnit.Percent;
        }

        public void ResetSlider() {
            _currentAnimationTween?.Kill();
            _loadingBarPercentage.PercentLoaded.value = ZERO_INT;
            _loadingBarPercentage.PercentLoaded.unit = LengthUnit.Percent;
        }

        public async Awaitable SetLoadingSlider(float valueBetween0To1, CancellationTokenSource cancellationTokenSource) {
            _currentAnimationTween?.Kill();
            _currentAnimationTween = _loadingBarPercentage.DOValue(valueBetween0To1, _animationDuration).SetEase(_animationEase);
            await _currentAnimationTween.WithCancellationSafe(cancellationToken: cancellationTokenSource.Token);
        }

        public void Show() {
            _loadingUiDocument.enabled = true;
        }

        public void Hide() {
            _loadingUiDocument.enabled = false;
        }
    }
}
