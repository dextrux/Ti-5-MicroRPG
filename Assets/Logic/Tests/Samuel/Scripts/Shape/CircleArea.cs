using UnityEngine;

public class CircleArea : AreaShape
{
    private float _radius;

    public CircleArea(float radius)
    {
        _radius = radius;
    }

    public override bool IsInArea(Vector2 center, Vector2 direction, Vector2 target)
    {
        Debug.Log("Center: " + center);
        Debug.Log("Target: " + target);
        return Vector2.Distance(center, target) <= _radius;
    }
}
