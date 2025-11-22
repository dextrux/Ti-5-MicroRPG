using Logic.Scripts.Services.AddressablesLoader;
using Logic.Scripts.Services.Logger.Base;
using System.Threading;
using UnityEngine;

public class LevelScenarioController : ILevelScenarioController {
    private readonly ILevelsDataService _levelsDataService;
    private readonly LevelFactory _levelFactory;

    private LevelTrackData _currentLevelScenarioData;
    public LevelScenarioView CurrentLevelTrackView => _currentLevelScenarioData.ScenarioView;

    public LevelScenarioController(IAddressablesLoaderService addressablesLoaderService, ILevelsDataService levelsDataService) {
        _levelsDataService = levelsDataService;
        _levelFactory = new LevelFactory(addressablesLoaderService);
    }

    public async Awaitable CreateLevelScenario(int levelNumber, CancellationTokenSource cancellationTokenSource) {
        Debug.LogWarning("CreateLevelScenario is null: " + (_levelsDataService == null));
        var trackAddress = _levelsDataService.GetLevelData(levelNumber).LevelAddress;
        LogService.LogTopic($"Create level {levelNumber} track , track adress: {trackAddress}", LogTopicType.LevelTrack);
        _currentLevelScenarioData = new LevelTrackData(await _levelFactory.CreateLevelTrack(trackAddress, cancellationTokenSource), trackAddress);
    }

    public void DestroyScenario(bool shouldReleaseFromMemory) {
        Object.Destroy(_currentLevelScenarioData.ScenarioView.gameObject);

        if (shouldReleaseFromMemory) {
            ReleaseCurrentLevelTrackFromMemory(_currentLevelScenarioData.ScenarioAddress);
        }
    }

    private void ReleaseCurrentLevelTrackFromMemory(string scenarioAddress) {
        _levelFactory.ReleaseTrackFromMemory(scenarioAddress);
    }

    private class LevelTrackData {
        public readonly LevelScenarioView ScenarioView;
        public readonly string ScenarioAddress;

        public LevelTrackData(LevelScenarioView levelScenarioView, string scenarioAddress) {
            ScenarioView = levelScenarioView;
            ScenarioAddress = scenarioAddress;
        }
    }
}
