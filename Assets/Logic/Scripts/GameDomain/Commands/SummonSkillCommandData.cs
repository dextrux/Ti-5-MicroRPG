using Logic.Scripts.Turns;
using UnityEngine;

public class SummonSkillCommandData : MonoBehaviour {
    public IEnvironmentTurnActor SummonedObject;

    public SummonSkillCommandData(IEnvironmentTurnActor summonedObject) {
        SummonedObject = summonedObject;
    }
}
