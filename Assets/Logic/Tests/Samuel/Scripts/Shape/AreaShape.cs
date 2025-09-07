using UnityEngine;

public abstract class AreaShape
{
    public abstract bool IsInArea(Vector2 center, Vector2 direction, Vector2 target);
}
