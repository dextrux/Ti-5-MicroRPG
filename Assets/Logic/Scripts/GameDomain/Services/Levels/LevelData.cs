using UnityEngine;
using System;
public abstract class LevelData : ScriptableObject {
    [SerializeField] private string levelAddress;
    public string LevelAddress => levelAddress;

    [SerializeField] private NaraMovementControllerReference controller;
    public System.Type ControllerType => controller?.ControllerType;
}
