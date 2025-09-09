using UnityEngine;

public abstract class AreaShape
{
    //protected Vector2 coords;

    public bool IsInArea(Vector2 center, Vector2 direction, Vector2 target)
    {
        //coords = center;

        return CalculateArea(center, direction, target);
    }   

    protected abstract bool CalculateArea(Vector2 center, Vector2 direction, Vector2 target);

    public abstract void VisualGizmo(Vector2 center, Vector2 direction, Vector2 target, ArenaPosReference arena);

    #region // Utilities

    protected float GetAngle(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360f;

        return angle;
    }

    protected Vector2 RotateArenaPoint(Vector2 center, Vector2 point, float angle)
    {
        Quaternion rot = Quaternion.Euler(0, 0, angle);
        return center + (Vector2)(rot * (point - center));

        /*float rad = angle * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);

        Vector2 p = point - center;
        float x = p.x * cos - p.y * sin;
        float y = p.x * sin + p.y * cos;

        return new Vector2(x, y) + center;*/
    }

    protected Vector3 RotateWorldPoint(Vector3 center, Vector3 point, float angle)
    {
        Quaternion rot = Quaternion.Euler(0, angle, 0);
        return center + (rot * (point - center));

        /*float rad = angle * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);

        Vector3 p = point - center;
        float x = p.x * cos - p.z * sin;
        float z = p.x * sin + p.z * cos;

        return new Vector3(x, point.y, z) + new Vector3(center.x, 0, center.y);*/
    }

    #endregion
}
