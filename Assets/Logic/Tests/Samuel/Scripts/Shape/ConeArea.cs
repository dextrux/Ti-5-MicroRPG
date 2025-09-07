using UnityEngine;

public class ConeArea : AreaShape
{
    private float _radius;
    private float _angle;

    public ConeArea(float radius, float angle)
    {
        _radius = radius;
        _angle = angle;
    }

    public override bool IsInArea(Vector2 center, Vector2 direction, Vector2 target)
    {
        if (!(Vector2.Distance(center, target) <= _radius)) return false;

        float dot = Vector2.Dot(direction, (target - center).normalized);

        Debug.Log(direction);
        Debug.Log((target - center).normalized);
        Debug.Log(dot);
        return true;
    }
}
