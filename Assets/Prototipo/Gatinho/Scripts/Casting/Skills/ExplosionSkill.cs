using System.Collections.Generic;
using UnityEngine;

public class ExplosionSkill : PlayerSkill
{
    protected override void Effect(List<Collider> colliders)
    {
        Debug.Log("EXPLODE!");
        Destroy(gameObject);
    }
}
