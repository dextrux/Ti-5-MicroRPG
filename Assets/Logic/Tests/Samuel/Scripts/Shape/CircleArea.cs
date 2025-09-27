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

    public override Vector3[] GetPoints(Vector2 center, Vector2 direction, ArenaPosReference arena)
    {
        float angle = GetAngle(direction);
        Vector2 pivot = RotateArenaPoint(center, center + centerPivot, -angle);
        Vector3 pos = arena.RelativeArenaPositionToRealPosition(pivot);

        int sides = 36;
        float angleSteps = 360f / sides;
        Vector3[] points = new Vector3[sides];
        for (int i = 0; i < sides; i++)
        {
            float currentAngle = i * angleSteps * Mathf.Deg2Rad;
            points[i] = new Vector3(Mathf.Cos(currentAngle) * _radius, 0, Mathf.Sin(currentAngle) * _radius) + pos;
        }

        return points;
    }
}
