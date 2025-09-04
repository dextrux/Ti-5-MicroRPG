using System.Threading;
using UnityEngine;

namespace Logic.Scripts.Core.Mvc.LoadingScreen
{
    public interface ILoadingScreenController
    {
        void Show();
        void Hide();
        void ResetSlider();
        Awaitable SetLoadingSlider(float valueBetween0To1, CancellationTokenSource cancellationTokenSource);
    }
}