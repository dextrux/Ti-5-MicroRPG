using UnityEngine;

public class SectionedArea : AreaShape
{
    private AreaShape _adderArea;
    private AreaShape _reducerArea;

    public SectionedArea(AreaShape adderArea, AreaShape reducerArea, Vector2 pivot) : base(pivot)
    {
        _adderArea = adderArea;
        _reducerArea = reducerArea;
    }

    protected override bool CalculateArea(Vector2 center, Vector2 direction, Vector2 target)
    {
        if (_adderArea == null || _reducerArea == null) return false;

        float angle = GetAngle(direction);
        Vector2 pivot = RotateArenaPoint(center, center, -angle);

        return _adderArea.IsInArea(pivot, direction, target) && !_reducerArea.IsInArea(pivot, direction, target);
    }

    public override void VisualGizmo(Vector2 center, Vector2 direction, ArenaPosReference arena, Color color)
    {
        if (_adderArea == null || _reducerArea == null) return;

        float angle = GetAngle(direction);
        Vector2 pivot = RotateArenaPoint(center, center + centerPivot, -angle);

        _adderArea.VisualGizmo(pivot, direction, arena, color);
        _reducerArea.VisualGizmo(pivot, direction, arena, Color.red);
    }

    public override Vector3[] GetPoints(Vector2 center, Vector2 direction, ArenaPosReference arena)
    {
        Vector3[] A = _adderArea.GetPoints(center, direction, arena);
        Vector3[] B = _reducerArea.GetPoints(center, direction, arena);

        Vector3[] points = new Vector3[A.Length + B.Length];
        for (int i = 0; i < A.Length; i++)
        {
            points[i] = A[i];
        }
        for (int i = A.Length; i < points.Length; i++)
        {
            points[i] = B[i - A.Length];
        }

        return points;
    }
}
