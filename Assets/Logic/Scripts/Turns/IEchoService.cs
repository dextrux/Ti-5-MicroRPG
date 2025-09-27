using System.Threading.Tasks;

namespace Logic.Scripts.Turns
{
    public interface IEchoService
    {
        void EnqueueEcho(IEchoAction action, int delayTurns);
        void ResolveDueEchoes();
        Task ResolveDueEchoesAsync();
        int PendingCount { get; }
    }
}
