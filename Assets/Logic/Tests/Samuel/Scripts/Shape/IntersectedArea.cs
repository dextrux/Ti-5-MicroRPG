using UnityEngine;

public class IntersectedArea : AreaShape
{
    private AreaShape _areaA;
    private AreaShape _areaB;

    public IntersectedArea(AreaShape areaA, AreaShape areaB, Vector2 pivot) : base(pivot)
    {
        _areaA = areaA;
        _areaB = areaB;
    }

    protected override bool CalculateArea(Vector2 center, Vector2 direction, Vector2 target, ArenaPosReference arena)
    {
        if (_areaA == null || _areaB == null) return false;

        float angle = GetAngle(direction);
        Vector2 pivot = RotateArenaPoint(center, center + centerPivot, -angle);

        return _areaA.IsInArea(pivot, direction, target,arena) && _areaB.IsInArea(pivot, direction, target, arena);
    }

    public override void VisualGizmo(Vector2 center, Vector2 direction, ArenaPosReference arena)
    {
        VisualGizmo(center, direction, arena, Color.green);
    }

    public override void VisualGizmo(Vector2 center, Vector2 direction, ArenaPosReference arena, Color color)
    {
        if (_areaA == null || _areaB == null) return;

        float angle = GetAngle(direction);
        Vector2 pivot = RotateArenaPoint(center, center + centerPivot, -angle);

        _areaA.VisualGizmo(pivot, direction, arena, color);
        _areaB.VisualGizmo(pivot, direction, arena, color);
    }
}
