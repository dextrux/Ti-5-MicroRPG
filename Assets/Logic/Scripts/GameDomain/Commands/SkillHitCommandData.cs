using Logic.Scripts.GameDomain.MVC.Abilitys;

public class SkillHitCommandData
{
    public AbilityData AbilityData;
    public IEffectable Caster;
    public IEffectable Target;

    public SkillHitCommandData(AbilityData abilityData, IEffectable caster, IEffectable target) {
        AbilityData = abilityData;
        Caster = caster;
        Target = target;
    }
}
