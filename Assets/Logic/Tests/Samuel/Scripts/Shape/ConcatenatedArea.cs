using UnityEngine;

public class ConcatenatedArea : AreaShape
{
    private AreaShape _areaA;
    private AreaShape _areaB;

    public ConcatenatedArea(AreaShape areaA, AreaShape areaB, Vector2 pivot) : base(pivot)
    {
        _areaA = areaA;
        _areaB = areaB;
    }

    protected override bool CalculateArea(Vector2 center, Vector2 direction, Vector2 target)
    {
        if (_areaA == null || _areaB == null) return false;

        float angle = GetAngle(direction);
        Vector2 pivot = RotateArenaPoint(center, center + centerPivot, -angle);

        return _areaA.IsInArea(pivot, direction, target) || _areaB.IsInArea(pivot, direction, target);
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
