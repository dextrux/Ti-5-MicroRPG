using System.Threading;
using UnityEngine;

namespace Logic.Scripts.Core.Mvc.LoadingScreen {
    public class LoadingScreenView : MonoBehaviour {
        //Inserir LoadingBard
        [SerializeField] private Canvas _loadingScreenCanvas;

        public void ResetSlider() {
            //Inserir fun��o reset da LoadingBard
        }

        public async Awaitable SetLoadingSlider(float valueBetween0To1, CancellationTokenSource cancellationTokenSource) {
            //await fun��o da loading bar AnimateSliderTo(valueBetween0To1, cancellationTokenSource);
        }

        public void Show() {
            _loadingScreenCanvas.enabled = true;
        }

        public void Hide() {
            _loadingScreenCanvas.enabled = false;
        }
    }
}
