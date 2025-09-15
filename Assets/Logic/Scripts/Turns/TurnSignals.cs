namespace Logic.Scripts.Turns
{
    public struct RequestEnterTurnModeSignal {}
    public struct RequestExitTurnModeSignal {}
    public struct EnterTurnModeSignal {}
    public struct ExitTurnModeSignal {}

    public struct TurnAdvancedSignal
    {
        public int TurnNumber;
        public TurnPhase Phase;
    }

    public struct BossActionRequestedSignal {}
    public struct BossActionCompletedSignal {}

    public struct RequestPlayerActionSignal {}
    public struct PlayerActionCompletedSignal {}
    public struct TurnSkippedSignal {}

    public struct EchoesResolutionRequestedSignal {}
    public struct EchoesResolutionCompletedSignal {}

    public struct EnviromentActionRequestedSignal {}
    public struct EnviromentActionCompletedSignal {}

    public struct ActionPointsChangedSignal
    {
        public int Current;
        public int Max;
    }
}
