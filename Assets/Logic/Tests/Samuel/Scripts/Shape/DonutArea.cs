using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class DonutArea : AreaShape
{
    private float _externalRadius;
    private float _internalRadius;

    public DonutArea(float majorRdius, float minorRadius)
    {
        _externalRadius = majorRdius;
        _internalRadius = minorRadius;
    }

    protected override bool CalculateArea(Vector2 center, Vector2 direction, Vector2 target)
    {
        float dis = Vector2.Distance(center, target);
        return (dis <= _externalRadius) && (dis >= _internalRadius);
    }

    public override void VisualGizmo(Vector2 center, Vector2 direction, Vector2 target, ArenaPosReference arena)
    {
#if UNITY_EDITOR
        Handles.color = Color.yellow;

        Vector3 pos = arena.RelativeArenaPositionToRealPosition(center);
        Handles.DrawWireDisc(pos, Vector3.up, _externalRadius);

        Handles.color = Color.red;

        Handles.DrawWireDisc(pos, Vector3.up, _internalRadius);

        Handles.color = Color.white;
#endif
    }
}
