using UnityEngine;

public class HealEffect : SkillEffect
{
    private int _heal;

    public HealEffect(int heal)
    {
        _heal = heal;
    }

    public override void Effect(EffectTarget_TEST target, EffectParameter_TEST parameterss)
    {
        Debug.Log("HEAL: " + _heal);
    }
}
