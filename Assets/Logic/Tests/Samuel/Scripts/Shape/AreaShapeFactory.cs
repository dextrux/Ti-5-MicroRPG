using System;
using UnityEngine;

[Serializable]
public class AreaShapeFactory
{
    public enum Shape
    {
        None,
        Circle,
        Cone,
        Donut,
        Square,
        Triagle,
        HalfMoon
    }

    public Shape shape;

    [Min(0.1f)] public float externalRadius = 1.5f;
    [Min(0.1f)] public float internalRadius = 1f;
    [Min(0.1f)] public float angle = 1f;

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
            case Shape.Donut:
                areaShape = CreateDonut();
                break;
            case Shape.Square:
                areaShape = CreateSquare();
                break;
            /*case Shape.Triagle:
                areaShape = CreateSquare();
                break;*/
            case Shape.HalfMoon:
                areaShape = CreateHalfMoon();
                break;
            default:
                return null;
        }

        return areaShape;
    }

    private AreaShape CreateCircle()
    {
        return new CircleArea(externalRadius);
    }

    private AreaShape CreateCone()
    {
        return new ConeArea(externalRadius, angle);
    }

    private AreaShape CreateDonut()
    {
        return new DonutArea(externalRadius, internalRadius);
    }

    private AreaShape CreateSquare()
    {
        return new SquareArea(height, width);
    }

    /*private AreaShape CreateTriangle()
    {

    }*/

    private AreaShape CreateHalfMoon()
    {
        return new HalfMoonArea(externalRadius, internalRadius, pivot);
    }
}
