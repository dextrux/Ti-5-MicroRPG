using Logic.Scripts.GameDomain.MVC.Abilitys;
using UnityEngine;

public class SkillHitCommandData
{
    public AbilityData AbilityData;
    public GameObject Caster;
    public GameObject Target;

    public SkillHitCommandData(AbilityData abilityData) {
        AbilityData = abilityData;
    }
}
