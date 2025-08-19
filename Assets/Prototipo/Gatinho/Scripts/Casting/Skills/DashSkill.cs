using System.Collections.Generic;
using UnityEngine;

public class DashSkill : PlayerSkill
{
    protected override void Effect(List<Collider> colliders)
    {
        Debug.Log("DASH!");
        Destroy(gameObject);
    }
}
