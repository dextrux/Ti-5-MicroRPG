using UnityEngine;

public abstract class BossAttack : MonoBehaviour
{
    [SerializeField] protected CompositedAreaShapeFactory _areaShapeFactory;
    protected AreaShape effectArea;

    private ArenaPosReference _arena;

    protected virtual void Awake()
    {
        effectArea = _areaShapeFactory.CreateAreaShape();

        Setup();
    }

    public void Prepare(ArenaPosReference arena)
    {
        _arena = arena;
    }

    public void Execute()
    {
        if (effectArea.IsInArea(_arena.RealPositionToRelativeArenaPosition(transform), new Vector2(transform.forward.x, transform.forward.z), _arena.GetPlayerArenaPosition(), _arena))
        {
            Effect();
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        if (effectArea == null) return;

        effectArea.VisualGizmo(_arena.RealPositionToRelativeArenaPosition(transform), new Vector2(transform.forward.x, transform.forward.z), _arena);
    }

    protected virtual void Setup() { }

    protected abstract void Effect();
}
