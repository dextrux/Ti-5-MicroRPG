using UnityEngine;
using UnityEngine.UI;
using Zenject;
using TMPro;
using Logic.Scripts.Turns;

namespace Logic.Scripts.Turns.Ui
{
    public class TurnHud : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _turnText;
        [SerializeField] private TextMeshProUGUI _phaseText;
        [SerializeField] private TextMeshProUGUI _apText;

        private ITurnEventBus _bus;

        private int _turn;
        private TurnPhase _phase;
        private int _apCurrent;
        private int _apMax;

        private string _turnDisplay = "-";
        private string _phaseDisplay = "none";

        [Inject]
        public void Construct(ITurnEventBus bus)
        {
            _bus = bus;
        }

        private void OnEnable()
        {
            _bus.Subscribe<TurnAdvancedSignal>(OnTurnAdvanced);
            _bus.Subscribe<ActionPointsChangedSignal>(OnApChanged);
            _bus.Subscribe<ExitTurnModeSignal>(OnExitTurnMode);
            Refresh();
        }

        private void OnDisable()
        {
            _bus.Unsubscribe<TurnAdvancedSignal>(OnTurnAdvanced);
            _bus.Unsubscribe<ActionPointsChangedSignal>(OnApChanged);
            _bus.Unsubscribe<ExitTurnModeSignal>(OnExitTurnMode);
        }

        private void OnTurnAdvanced(TurnAdvancedSignal s)
        {
            _turn = s.TurnNumber;
            _phase = s.Phase;
            _turnDisplay = _turn.ToString();
            _phaseDisplay = _phase.ToString();
            Refresh();
        }

        private void OnApChanged(ActionPointsChangedSignal s)
        {
            _apCurrent = s.Current;
            _apMax = s.Max;
            Refresh();
        }

        private void OnExitTurnMode(ExitTurnModeSignal _)
        {
            _turnDisplay = "-";
            _phaseDisplay = "none";
            Refresh();
        }

        private void Refresh()
        {
            if (_turnText) _turnText.text = $"Turno: {_turnDisplay}";
            if (_phaseText) _phaseText.text = $"Fase: {_phaseDisplay}";
            if (_apText) _apText.text = $"AP: {_apCurrent}/{_apMax}";
        }
    }
}
