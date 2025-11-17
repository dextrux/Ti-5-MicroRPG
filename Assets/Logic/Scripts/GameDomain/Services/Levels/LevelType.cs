using UnityEngine;
public abstract class LevelType {
    [field: SerializeField] public string LevelAddress { get; }
    [field: SerializeField] public string BossAddress { get; }
    [field: SerializeField] public INaraMovementController Controller { get; }
}
