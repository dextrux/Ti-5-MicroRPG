using Logic.Scripts.Core.CoreInitiator.Base;
public class ExplorationInitiatorEnterData : IInitiatorEnterData {
    public int LevelNumberToEnter;

    public ExplorationInitiatorEnterData(int levelNumberToEnter) {
        LevelNumberToEnter = levelNumberToEnter;
    }
}
