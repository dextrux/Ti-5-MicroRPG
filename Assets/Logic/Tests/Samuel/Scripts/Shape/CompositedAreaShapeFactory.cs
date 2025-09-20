using System;
using UnityEngine;

[Serializable]
public class CompositedAreaShapeFactory : AreaShapeFactory
{
    public AreaShapeFactory subFactory1 = new AreaShapeFactory();
    public AreaShapeFactory subFactory2 = new AreaShapeFactory();

    protected override AreaShape DefaultExceptionCreate()
    {
        AreaShape areaShape;
        switch (shape)
        {
            case Shape.Concatenated:
                areaShape = CreateConcatenated();
                break;
            case Shape.Intersected:
                areaShape = CreateIntersected();
                break;
            case Shape.Sectioned:
                areaShape = CreateSectioned();
                break;
            case Shape.Divergent:
                areaShape = CreateDivergent();
                break;
            default:
                return base.DefaultExceptionCreate();
        }

        return areaShape;
    }

    private AreaShape CreateConcatenated()
    {
        return new ConcatenatedArea(subFactory1.CreateAreaShape(), subFactory2.CreateAreaShape(), pivot);
    }

    private AreaShape CreateIntersected()
    {
        return new IntersectedArea(subFactory1.CreateAreaShape(), subFactory2.CreateAreaShape(), pivot);
    }

    private AreaShape CreateSectioned()
    {
        return new SectionedArea(subFactory1.CreateAreaShape(), subFactory2.CreateAreaShape(), pivot);
    }

    private AreaShape CreateDivergent()
    {
        return new DivergentArea(subFactory1.CreateAreaShape(), subFactory2.CreateAreaShape(), pivot);
    }
}
