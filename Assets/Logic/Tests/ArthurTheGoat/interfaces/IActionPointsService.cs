namespace Logic.Tests.ArthurTheGoat.Turns
{
    public interface IActionPointsService
    {
        int Current { get; }
        int Max { get; }
        int GainPerTurn { get; }
        bool CanSpend(int amount);
        bool Spend(int amount);
        void GainTurnPoints();
        void Refill();
        void Reset();
        void Configure(int max, int gainPerTurn);
    }
}


