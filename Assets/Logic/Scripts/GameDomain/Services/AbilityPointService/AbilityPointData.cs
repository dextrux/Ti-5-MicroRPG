using UnityEngine;

[CreateAssetMenu(fileName = "AbilityPointData", menuName = "Scriptable Objects/AbilityPointData")]
public class AbilityPointData : ScriptableObject
{
    public int StartAdvantagePoints;
    public int StartDisadvantagePoints;
    public int IncreaseAdvantagePoints;
    public int IncreaseDisadvantagePoints;
}
