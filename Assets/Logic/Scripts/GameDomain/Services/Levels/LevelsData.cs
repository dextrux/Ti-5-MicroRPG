using UnityEngine;

[CreateAssetMenu(fileName = "LevelsData", menuName = "Scriptable Objects/Levels/LevelsData")]
public class LevelsData : ScriptableObject {
    [SerializeReference] public LevelData[] AllLevels;
}
