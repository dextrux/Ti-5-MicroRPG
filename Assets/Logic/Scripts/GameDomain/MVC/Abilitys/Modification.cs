using System;
using UnityEngine;

[Serializable]
public class Modification
{
    [SerializeField] public ModificationType Type;
    [SerializeField] public int Value;
}
