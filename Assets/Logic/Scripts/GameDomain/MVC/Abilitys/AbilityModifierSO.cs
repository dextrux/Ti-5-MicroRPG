using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AbilityModifierSO", menuName = "Scriptable Objects/AbilityModifierSO")]
public class AbilityModifierSO : ScriptableObject
{
    public Dictionary<Modification, int> Modification = new Dictionary<Modification, int>();
}