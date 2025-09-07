using UnityEngine;

public abstract class BossAttack : MonoBehaviour
{
    protected AreaShape effectArea;

    public void Execute(ArenaPosReference arena)
    {
        if (effectArea.IsInArea(arena.GetArenaRelativePosition(transform), transform.forward, arena.GetPlayerArenaPosition()))
        {
            Effect();
        }

        Destroy(gameObject);
    }

    protected abstract void Effect();
}
