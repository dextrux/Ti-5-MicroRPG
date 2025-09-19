using Zenject;

namespace Logic.Scripts.Turns
{
    public class ActionPointsService : IActionPointsService
    {
        private readonly ITurnEventBus _eventBus;

        private int _current;
        private int _max;
        private int _gainPerTurn;

        public int Current => _current;
        public int Max => _max;
        public int GainPerTurn => _gainPerTurn;

        public ActionPointsService(ITurnEventBus eventBus)
        {
            _eventBus = eventBus;
            _max = 10;
            _gainPerTurn = 2;
            _current = 0;
            PublishChange();
        }

        public void Configure(int max, int gainPerTurn)
        {
            _max = max < 0 ? 0 : max;
            _gainPerTurn = gainPerTurn < 0 ? 0 : gainPerTurn;
            if (_current > _max) _current = _max;
            PublishChange();
        }

        public bool CanSpend(int amount)
        {
            if (amount <= 0) return true;
            return _current >= amount;
        }

        public bool Spend(int amount)
        {
            if (amount <= 0) return true;
            if (_current < amount) return false;
            _current -= amount;
            PublishChange();
            return true;
        }

        public void GainTurnPoints()
        {
            _current += _gainPerTurn;
            if (_current > _max) _current = _max;
            PublishChange();
        }

        public void Refill()
        {
            _current = _max;
            PublishChange();
        }

        public void Reset()
        {
            _current = 0;
            PublishChange();
        }

        private void PublishChange()
        {
            _eventBus.Publish(new ActionPointsChangedSignal { Current = _current, Max = _max });
        }
    }
}
