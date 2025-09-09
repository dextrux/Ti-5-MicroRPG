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

    protected override bool CalculateArea(Vector2 center, Vector2 direction, Vector2 target, ArenaPosReference arena)
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
}
