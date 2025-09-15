using Logic.Scripts.Core.CoreInitiator.Base;

public class GamePlayInitatorEnterData : IInitiatorEnterData {
    public int LevelNumberToEnter;

    public GamePlayInitatorEnterData(int levelNumberToEnter) {
        LevelNumberToEnter = levelNumberToEnter;
    }
}
