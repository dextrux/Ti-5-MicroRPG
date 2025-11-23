using Logic.Scripts.Services.Logger.Base;

public class GamePlayDataService : IGamePlayDataService {
    public int CurrentLevelNumber { get; private set; }
    public void SetCurrentLevelNumber(int levelNumber) {
        LogService.LogTopic($"Set current level number to {levelNumber}", LogTopicType.GamePlayData);
        CurrentLevelNumber = levelNumber;
    }
}
