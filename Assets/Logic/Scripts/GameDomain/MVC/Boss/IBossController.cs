namespace Logic.Scripts.GameDomain.MVC.Boss
{
    public interface IBossController
    {
        void Initialize();
        void PlanNextTurn();
        void ExecuteTurn();
        bool IsCasting { get; }
        int RemainingCastTurns { get; }
    }
}
