using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class CircleArea : AreaShape
{
    private float _radius;

    public CircleArea(float radius)
    {
        _radius = radius;
    }

    protected override bool CalculateArea(Vector2 center, Vector2 direction, Vector2 target)
    {
        return Vector2.Distance(center, target) <= _radius;
    }

    public override void VisualGizmo(Vector2 center, Vector2 direction, Vector2 target, ArenaPosReference arena)
    {
#if UNITY_EDITOR
        Handles.color = Color.yellow;

        Handles.DrawWireDisc(arena.RelativeArenaPositionToRealPosition(center), Vector3.up, _radius);

        Handles.color = Color.white;
#endif
    }
}
