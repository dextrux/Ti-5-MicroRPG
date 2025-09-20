using System;
using UnityEngine;

[Serializable]
public class SkillEffectFactory
{
    public enum Effect
    {
        None,
        Damage,
        Heal
    }

    public Effect effect;

    [Min(0)] public int intValue;

    public SkillEffect CreateEffect()
    {
        SkillEffect skillEffect;
        switch (effect)
        {
            case Effect.Damage:
                skillEffect = CreateDamage();
                break;
            case Effect.Heal:
                skillEffect = CreateHeal();
                break;
            default:
                return null;
        }

        return skillEffect;
    }

    #region // Effects

    private SkillEffect CreateDamage()
    {
        return new DamageEffect(intValue);
    }

    private SkillEffect CreateHeal()
    {
        return new HealEffect(intValue);
    }

    #endregion
}
