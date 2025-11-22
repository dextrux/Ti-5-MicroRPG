using System.Threading;
public interface ILevelCancellationTokenService {
    CancellationTokenSource CancellationTokenSource { get; }
    void InitCancellationToken();
    void CancelCancellationToken();
}
