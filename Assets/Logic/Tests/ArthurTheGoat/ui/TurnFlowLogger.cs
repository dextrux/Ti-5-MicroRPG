using UnityEngine;
using Zenject;

namespace Logic.Tests.ArthurTheGoat.Turns
{
    public class TurnFlowLogger : MonoBehaviour
    {
        ITurnEventBus _bus;

        [Inject]
        public void Construct(ITurnEventBus bus)
        {
            _bus = bus;
        }

        void OnEnable()
        {
            _bus.Subscribe<TurnAdvancedSignal>(OnTurnAdvanced);
            _bus.Subscribe<ActionPointsChangedSignal>(OnApChanged);
            _bus.Subscribe<RequestPlayerActionSignal>(OnPlayerActionRequested);
            _bus.Subscribe<EchoesResolutionRequestedSignal>(_ => Debug.Log("Ecos: resolver"));
            _bus.Subscribe<EchoesResolutionCompletedSignal>(_ => Debug.Log("Ecos: concluído"));
        }

        void OnDisable()
        {
            _bus.Unsubscribe<TurnAdvancedSignal>(OnTurnAdvanced);
            _bus.Unsubscribe<ActionPointsChangedSignal>(OnApChanged);
            _bus.Unsubscribe<RequestPlayerActionSignal>(OnPlayerActionRequested);
            _bus.Unsubscribe<EchoesResolutionRequestedSignal>(_ => Debug.Log("Ecos: resolver"));
            _bus.Unsubscribe<EchoesResolutionCompletedSignal>(_ => Debug.Log("Ecos: concluído"));
        }

        void OnTurnAdvanced(TurnAdvancedSignal s)
        {
            Debug.Log($"Turno {s.TurnNumber} - Fase: {s.Phase}");
        }

        void OnApChanged(ActionPointsChangedSignal s)
        {
            Debug.Log($"AP: {s.Current}/{s.Max}");
        }

        void OnPlayerActionRequested(RequestPlayerActionSignal _)
        {
            Debug.Log("Player: ação requerida");
        }
    }
}


