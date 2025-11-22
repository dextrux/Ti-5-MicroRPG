using Logic.Scripts.GameDomain.MVC.Boss;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelTurnData", menuName = "Scriptable Objects/Levels/LevelTurnData")]
public class LevelTurnData : LevelData {
    [SerializeField] private BossConfigurationSO bossConfiguration;
    public BossConfigurationSO BossConfiguration => bossConfiguration;

    [SerializeField] private BossView bossPrefab;
    public BossView BossPrefab => bossPrefab;
}
