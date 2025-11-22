using System.Threading;
using UnityEngine;

public interface ILevelScenarioController {
    Awaitable CreateLevelScenario(int levelNumber, CancellationTokenSource cancellationTokenSource);
    void DestroyScenario(bool shouldReleaseFromMemory);
    LevelScenarioView CurrentLevelTrackView { get; }
}
