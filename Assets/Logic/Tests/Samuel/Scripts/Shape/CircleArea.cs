using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class CircleArea : AreaShape
{
    private float _radius;

    public CircleArea(float radius, Vector2 pivot) : base(pivot)
    {
        _radius = radius;
    }

    protected override bool CalculateArea(Vector2 center, Vector2 direction, Vector2 target)
    {
        return Vector2.Distance(center, target) <= _radius;
    }

    public override void VisualGizmo(Vector2 center, Vector2 direction, ArenaPosReference arena, Color color)
    {
        float angle = GetAngle(direction);
        Vector2 pivot = RotateArenaPoint(center, center + centerPivot, -angle);

#if UNITY_EDITOR
        Handles.color = color;

        Handles.DrawWireDisc(arena.RelativeArenaPositionToRealPosition(pivot), Vector3.up, _radius);

        Handles.color = Color.white;
#endif
    }
}
