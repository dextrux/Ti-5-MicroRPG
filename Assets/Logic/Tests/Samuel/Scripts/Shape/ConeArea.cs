using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ConeArea : AreaShape
{
    private float _radius;
    private float _angle;

    public ConeArea(float radius, float angle, Vector2 pivot) : base(pivot)
    {
        _radius = radius;
        _angle = angle;
    }

    protected override bool CalculateArea(Vector2 center, Vector2 direction, Vector2 target)
    {
        if (!(Vector2.Distance(center, target) <= _radius)) return false;

        float dot = Vector2.Dot(direction, (target - center).normalized);

        return dot > Mathf.Cos((_angle / 2) * Mathf.Deg2Rad);
    }

    public override void VisualGizmo(Vector2 center, Vector2 direction, ArenaPosReference arena, Color color)
    {
        float angle = GetAngle(direction);
        Vector2 pivot = RotateArenaPoint(center, center + centerPivot, -angle);

        Vector3 pos = arena.RelativeArenaPositionToRealPosition(pivot);

        Vector3 worldDirection = new Vector3(direction.x, 0f, direction.y);

        Vector3 fowardPoint = pos + (worldDirection.normalized * _radius);
        Vector3 angledPoint1 = RotateWorldPoint(pos, fowardPoint, -(_angle / 2));
        Vector3 angledPoint2 = RotateWorldPoint(pos, fowardPoint, (_angle / 2));

#if UNITY_EDITOR
        Handles.color = color;

        Handles.DrawWireArc(pos, Vector3.up, (angledPoint1 - pos).normalized, _angle, _radius);

        Handles.color = Color.white;
#endif

        Gizmos.color = color;

        Gizmos.DrawLine(pos, angledPoint1);
        Gizmos.DrawLine(pos, angledPoint2);

        Gizmos.color = Color.white;
    }

    public override Vector3[] GetPoints(Vector2 center, Vector2 direction, ArenaPosReference arena)
    {
        float angle = GetAngle(direction);
        Vector2 pivot = RotateArenaPoint(center, center + centerPivot, -angle);
        Vector3 pos = arena.RelativeArenaPositionToRealPosition(pivot);

        Vector3 worldDirection = new Vector3(direction.x, 0f, direction.y);

        Vector3 fowardPoint = pos + (worldDirection.normalized * _radius);
        Vector3 angledPoint1 = RotateWorldPoint(pos, fowardPoint, -(_angle / 2));
        Vector3 angledPoint2 = RotateWorldPoint(pos, fowardPoint, (_angle / 2));

        int sides = 36;
        float angleSteps = _angle / (float)36;
        Vector3[] points = new Vector3[sides + 2];
        for (int i = 0; i < sides; i++)
        {
            float currentAngle = ((i * angleSteps) + _angle/2) * Mathf.Deg2Rad;
            points[i] = new Vector3(Mathf.Cos(currentAngle) * _radius, 0, Mathf.Sin(currentAngle) * _radius) + pos;
        }

        points[sides] = pos;
        points[sides + 1] = angledPoint2;

        return points;
    }
}
