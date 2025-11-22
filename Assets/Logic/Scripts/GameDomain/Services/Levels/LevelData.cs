using UnityEngine;
using System;
using Logic.Scripts.GameDomain.MVC.Nara;
public abstract class LevelData : ScriptableObject {
    [SerializeField] private string levelAddress;
    public string LevelAddress => levelAddress;

    [SerializeField] private NaraConfigurationSO naraLevelConfiguration;
    public NaraConfigurationSO NaraLevelConfiguration => naraLevelConfiguration;

    [SerializeField] private NaraMovementControllerReference controller;
    public System.Type ControllerType => controller?.ControllerType;
}
