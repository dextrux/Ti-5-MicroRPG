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
        float angleStep = _angle / (float)sides;

        Vector3[] points = new Vector3[sides + 2];
        points[0] = pos;
        points[1] = angledPoint1;

        for (int i = 1; i < sides; i++)
        {
            float currentAngle = -(_angle * 0.5f) + (i * angleStep);
            points[1 + i] = RotateWorldPoint(pos, fowardPoint, currentAngle);
        }

        points[sides + 1 - 0] = angledPoint2; // index = sides + 1 - 0 == sides + 1

        return points;
    }

    public static Vector3[] GenerateConeArcVertices(Vector3 origin, Vector3 forward, float radius, float angleDeg, int sides)
    {
        int clampedSides = sides < 1 ? 1 : sides;
        Vector3 planarForward = new Vector3(forward.x, 0f, forward.z);
        if (planarForward.sqrMagnitude < 1e-6f) planarForward = Vector3.forward;
        Vector3 basePoint = origin + planarForward.normalized * radius;
        float step = angleDeg / (float)clampedSides;
        Vector3[] arc = new Vector3[clampedSides + 1];
        for (int i = 0; i <= clampedSides; i++)
        {
            float currentAngle = -(angleDeg * 0.5f) + (i * step);
            Quaternion rot = Quaternion.Euler(0f, currentAngle, 0f);
            arc[i] = origin + (rot * (basePoint - origin));
        }
        return arc;
    }

    public static Vector3[] GenerateConeOutlinePolygon(Vector3 origin, Vector3 forward, float radius, float angleDeg, int sides)
    {
        Vector3[] arc = GenerateConeArcVertices(origin, forward, radius, angleDeg, sides);
        Vector3[] polygon = new Vector3[arc.Length + 1];
        polygon[0] = origin;
        for (int i = 0; i < arc.Length; i++)
        {
            polygon[i + 1] = arc[i];
        }
        return polygon;
    }

    public static bool IsPointInsideCone(Vector3 origin, Vector3 forward, float radius, float angleDeg, Vector3 point)
    {
        Vector3 planarForward = new Vector3(forward.x, 0f, forward.z);
        if (planarForward.sqrMagnitude < 1e-6f) return false;
        Vector3 toPoint = new Vector3(point.x - origin.x, 0f, point.z - origin.z);
        float distance = toPoint.magnitude;
        if (distance > radius) return false;
        float cosHalf = Mathf.Cos(0.5f * angleDeg * Mathf.Deg2Rad);
        float dot = Vector3.Dot(planarForward.normalized, toPoint.normalized);
        return dot > cosHalf;
    }
}
