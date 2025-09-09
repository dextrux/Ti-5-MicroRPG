using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class HalfMoonArea : AreaShape
{
    private float _externalRadius;
    private float _internalRadius;
    private Vector2 _centerPivot;

    public HalfMoonArea(float majorRdius, float minorRadius, Vector2 centerPivot)
    {
        _externalRadius = majorRdius;
        _internalRadius = minorRadius;
        _centerPivot = centerPivot;
    }

    protected override bool CalculateArea(Vector2 center, Vector2 direction, Vector2 target)
    {
        float mainDis = Vector2.Distance(center, target);

        float angle = GetAngle(direction);
        Vector2 pivot = RotateArenaPoint(center, center + _centerPivot, -angle);

        float pivotDis = Vector2.Distance(pivot, target);

        return (mainDis <= _externalRadius) && (pivotDis >= _internalRadius);
    }

    public override void VisualGizmo(Vector2 center, Vector2 direction, Vector2 target, ArenaPosReference arena)
    {
#if UNITY_EDITOR
        Vector3 pos = arena.RelativeArenaPositionToRealPosition(center);

        Handles.color = Color.yellow;

        Handles.DrawWireDisc(pos , Vector3.up, _externalRadius);

        Handles.color = Color.red;

        float angle = GetAngle(direction);
        Vector2 aux = RotateArenaPoint(center, center + _centerPivot, -angle);

        Handles.DrawWireDisc(arena.RelativeArenaPositionToRealPosition(aux), Vector3.up, _internalRadius);

        Handles.color = Color.white;
#endif
    }
}
