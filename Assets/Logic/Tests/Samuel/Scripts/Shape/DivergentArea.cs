using UnityEngine;

public class DivergentArea : AreaShape
{
    private AreaShape _areaA;
    private AreaShape _areaB;

    public DivergentArea(AreaShape areaA, AreaShape areaB, Vector2 pivot) : base(pivot)
    {
        _areaA = areaA;
        _areaB = areaB;
    }

    protected override bool CalculateArea(Vector2 center, Vector2 direction, Vector2 target, ArenaPosReference arena)
    {
        if (_areaA == null || _areaB == null) return false;

        float angle = GetAngle(direction);
        Vector2 pivot = RotateArenaPoint(center, center + centerPivot, -angle);

        bool validateA = _areaA.IsInArea(pivot, direction, target, arena);
        bool validateB = _areaB.IsInArea(pivot, direction, target, arena);
        return (validateA && !validateB) || (!validateA && validateB);
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
