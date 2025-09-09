using System;
using UnityEngine;

[Serializable]
public class AreaShapeFactory
{
    public enum Shape
    {
        None,
        Concatenated,
        Intersected,
        Sectioned,
        Divergent,
        Circle,
        Cone,
        Square,
        Triagle
    }

    public Shape shape;

    [Min(0.1f)] public float externalRadius = 1.5f;
    [Min(0.1f)] public float internalRadius = 1f;
    [Range(0.1f, 360f)] public float angle = 1f;

    [Min(0.1f)] public float height = 1f;
    [Min(0.1f)] public float width = 1f;

    public Vector2 pivot;

    public AreaShape CreateAreaShape()
    {
        AreaShape areaShape;
        switch (shape)
        {
            case Shape.Circle:
                areaShape = CreateCircle();
                break;
            case Shape.Cone:
                areaShape = CreateCone();
                break;
            case Shape.Square:
                areaShape = CreateSquare();
                break;
            /*case Shape.Triagle:
                areaShape = CreateSquare();
                break;*/           
            default:
                return DefaultExceptionCreate();
        }

        return areaShape;
    }

    #region // Shapes

    private AreaShape CreateCircle()
    {
        return new CircleArea(externalRadius, pivot);
    }

    private AreaShape CreateCone()
    {
        return new ConeArea(externalRadius, angle, pivot);
    }

    private AreaShape CreateSquare()
    {
        return new SquareArea(height, width, pivot);
    }

    /*private AreaShape CreateTriangle()
    {

    }*/

    #endregion

    protected virtual AreaShape DefaultExceptionCreate()
    {
        return null;
    }
}
