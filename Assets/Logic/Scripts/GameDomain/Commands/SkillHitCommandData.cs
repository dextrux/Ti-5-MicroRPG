using Logic.Scripts.GameDomain.MVC.Abilitys;
using UnityEngine;

public class SkillHitCommandData
{
    public AbilityData AbilityData;
    public IEffectable Caster;
    public IEffectable Target;

    public SkillHitCommandData(AbilityData abilityData) {
        AbilityData = abilityData;
    }
}
