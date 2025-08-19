using System.Collections.Generic;
using UnityEngine;

public class HealSkill : PlayerSkill
{
    protected override void Effect(List<Collider> colliders)
    {
        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out PlayerEchoAction auxGetPlayer))
            {
                Debug.Log("HEAL!");
                break;
            }
        }

        Destroy(gameObject);
    }
}
