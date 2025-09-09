using System.Collections.Generic;

namespace Logic.Tests.ArthurTheGoat.Turns
{
    public class EchoService : IEchoService
    {
        readonly List<EchoEntry> _entries = new List<EchoEntry>();
        readonly ITurnEventBus _bus;

        public EchoService(ITurnEventBus bus)
        {
            _bus = bus;
        }

        public int PendingCount => _entries.Count;

        public void EnqueueEcho(IEchoAction action, int delayTurns)
        {
            var turns = delayTurns < 0 ? 0 : delayTurns;
            _entries.Add(new EchoEntry(action, turns));
        }

        public async void ResolveDueEchoes()
        {
            await System.Threading.Tasks.Task.Delay(1000);

            for (int i = 0; i < _entries.Count; i++)
            {
                var e = _entries[i];
                e.TurnsRemaining -= 1;
                _entries[i] = e;
            }

            for (int i = _entries.Count - 1; i >= 0; i--)
            {
                if (_entries[i].TurnsRemaining <= 0)
                {
                    _entries[i].Action.Execute();
                    _entries.RemoveAt(i);
                }
            }

            _bus.Publish(new EchoesResolutionCompletedSignal());
        }

        struct EchoEntry
        {
            public IEchoAction Action;
            public int TurnsRemaining;

            public EchoEntry(IEchoAction action, int turnsRemaining)
            {
                Action = action;
                TurnsRemaining = turnsRemaining;
            }
        }
    }
}


