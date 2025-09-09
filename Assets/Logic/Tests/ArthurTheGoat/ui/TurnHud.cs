using UnityEngine;
using UnityEngine.UI;
using Zenject;
using TMPro;

namespace Logic.Tests.ArthurTheGoat.Turns
{
    public class TurnHud : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _turnText;
        [SerializeField] TextMeshProUGUI _phaseText;
        [SerializeField] TextMeshProUGUI _apText;

        ITurnEventBus _bus;

        int _turn;
        TurnPhase _phase;
        int _apCurrent;
        int _apMax;

        string _turnDisplay = "-";
        string _phaseDisplay = "none";

        [Inject]
        public void Construct(ITurnEventBus bus)
        {
            _bus = bus;
        }

        void OnEnable()
        {
            _bus.Subscribe<TurnAdvancedSignal>(OnTurnAdvanced);
            _bus.Subscribe<ActionPointsChangedSignal>(OnApChanged);
            _bus.Subscribe<ExitTurnModeSignal>(OnExitTurnMode);
            Refresh();
        }

        void OnDisable()
        {
            _bus.Unsubscribe<TurnAdvancedSignal>(OnTurnAdvanced);
            _bus.Unsubscribe<ActionPointsChangedSignal>(OnApChanged);
            _bus.Unsubscribe<ExitTurnModeSignal>(OnExitTurnMode);
        }

        void OnTurnAdvanced(TurnAdvancedSignal s)
        {
            _turn = s.TurnNumber;
            _phase = s.Phase;
            _turnDisplay = _turn.ToString();
            _phaseDisplay = _phase.ToString();
            Refresh();
        }

        void OnApChanged(ActionPointsChangedSignal s)
        {
            _apCurrent = s.Current;
            _apMax = s.Max;
            Refresh();
        }

        void OnExitTurnMode(ExitTurnModeSignal _)
        {
            _turnDisplay = "-";
            _phaseDisplay = "none";
            Refresh();
        }

        void Refresh()
        {
            if (_turnText) _turnText.text = $"Turno: {_turnDisplay}";
            if (_phaseText) _phaseText.text = $"Fase: {_phaseDisplay}";
            if (_apText) _apText.text = $"AP: {_apCurrent}/{_apMax}";
        }
    }
}


