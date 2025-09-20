using UnityEngine;

public class DamageEffect : SkillEffect
{
    private int _damage;

    public DamageEffect(int damage)
    {
        _damage = damage;
    }

    public override void Effect(EffectTarget_TEST target, EffectParameter_TEST parameters)
    {
        Debug.Log("DAMAGE: " + _damage);
    }
}
