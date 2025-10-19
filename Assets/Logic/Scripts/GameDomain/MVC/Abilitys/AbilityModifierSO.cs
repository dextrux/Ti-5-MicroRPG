using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AbilityModifierSO", menuName = "Scriptable Objects/AbilityModifierSO")]
public class AbilityModifierSO : ScriptableObject
{
    public string Name;
    public string Description;
    public List<Modification> Modifications;
}