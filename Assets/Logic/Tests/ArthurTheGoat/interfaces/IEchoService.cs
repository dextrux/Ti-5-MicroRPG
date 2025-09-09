namespace Logic.Tests.ArthurTheGoat.Turns
{
    public interface IEchoService
    {
        void EnqueueEcho(IEchoAction action, int delayTurns);
        void ResolveDueEchoes();
        int PendingCount { get; }
    }
}


