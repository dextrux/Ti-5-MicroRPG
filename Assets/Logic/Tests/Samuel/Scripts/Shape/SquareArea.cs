using UnityEngine;

public class SquareArea : AreaShape
{
    private float _height;
    private float _width;

    public SquareArea(float height, float width, Vector2 pivot) : base(pivot)
    {
        _height = height;
        _width = width;
    }

    protected override bool CalculateArea(Vector2 center, Vector2 direction, Vector2 target, ArenaPosReference arena)
    {
        float angle = GetAngle(direction);
        Vector2 newTargetPos = RotateArenaPoint(center, target, -angle);

        return (Mathf.Abs(newTargetPos.x - center.x) <= _width / 2) && (Mathf.Abs(newTargetPos.y - center.y) <= _height / 2);
    }

    public override void VisualGizmo(Vector2 center, Vector2 direction, ArenaPosReference arena, Color color)
    {
        float angle = GetAngle(direction);
        Vector2 pivot = RotateArenaPoint(center, center + centerPivot, -angle);

        Vector3 pos = arena.RelativeArenaPositionToRealPosition(pivot);
        Vector3 A = new Vector3(pos.x + _width / 2, pos.y, pos.z + _height / 2);
        Vector3 B = new Vector3(pos.x - _width / 2, pos.y, pos.z + _height / 2);
        Vector3 C = new Vector3(pos.x - _width / 2, pos.y, pos.z - _height / 2);
        Vector3 D = new Vector3(pos.x + _width / 2, pos.y, pos.z - _height / 2);

        A = RotateWorldPoint(pos, A, angle);
        B = RotateWorldPoint(pos, B, angle);
        C = RotateWorldPoint(pos, C, angle);
        D = RotateWorldPoint(pos, D, angle);

        Gizmos.color = color;

        Gizmos.DrawLine(A, B);
        Gizmos.DrawLine(B, C);
        Gizmos.DrawLine(C, D);
        Gizmos.DrawLine(D, A);

        Gizmos.color = Color.white;       
    }
}
