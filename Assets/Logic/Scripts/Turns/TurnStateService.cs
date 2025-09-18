using System;
using System.Collections.Generic;

namespace Logic.Scripts.Turns
{
    public interface ITurnStateReader
    {
        bool Active { get; }
        int TurnNumber { get; }
        TurnPhase Phase { get; }
        int ActionPointsCurrent { get; }
        int ActionPointsMax { get; }

        event Action EnteredTurnMode;
        event Action ExitedTurnMode;
        event Action<int, TurnPhase> TurnAdvanced;
        event Action<int, int> ActionPointsChanged;
        event Action<System.Collections.Generic.List<BarrierState>> BarrierStateChanged;
        event Action<System.Collections.Generic.List<HazardZoneState>> HazardZonesChanged;
        event Action PlayerActionRequested;
    }

    public class TurnStateService : ITurnStateReader
    {
        public bool Active { get; private set; }
        public int TurnNumber { get; private set; }
        public TurnPhase Phase { get; private set; }
        public int ActionPointsCurrent { get; private set; }
        public int ActionPointsMax { get; private set; }

        public event Action EnteredTurnMode;
        public event Action ExitedTurnMode;
        public event Action<int, TurnPhase> TurnAdvanced;
        public event Action<int, int> ActionPointsChanged;
        public event Action<List<BarrierState>> BarrierStateChanged;
        public event Action<List<HazardZoneState>> HazardZonesChanged;
        public event Action PlayerActionRequested;

        public void EnterTurnMode()
        {
            Active = true;
            TurnNumber = 0;
            Phase = TurnPhase.None;
            EnteredTurnMode?.Invoke();
        }

        public void ExitTurnMode()
        {
            Active = false;
            Phase = TurnPhase.None;
            ExitedTurnMode?.Invoke();
        }

        public void AdvanceTurn(int turnNumber, TurnPhase phase)
        {
            TurnNumber = turnNumber;
            Phase = phase;
            TurnAdvanced?.Invoke(TurnNumber, Phase);
        }

        public void UpdateActionPoints(int current, int max)
        {
            ActionPointsCurrent = current;
            ActionPointsMax = max;
            ActionPointsChanged?.Invoke(ActionPointsCurrent, ActionPointsMax);
        }

        public void PublishBarrierStateChanged(List<BarrierState> barriers)
        {
            BarrierStateChanged?.Invoke(barriers);
        }

        public void PublishHazardZonesChanged(List<HazardZoneState> zones)
        {
            HazardZonesChanged?.Invoke(zones);
        }

        public void RequestPlayerAction()
        {
            PlayerActionRequested?.Invoke();
        }
    }
}


