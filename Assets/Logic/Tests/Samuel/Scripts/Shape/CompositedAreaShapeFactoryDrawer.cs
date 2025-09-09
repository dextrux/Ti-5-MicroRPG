#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.Rendering;

[CustomPropertyDrawer(typeof(CompositedAreaShapeFactory))]
public class CompositedAreaShapeFactoryDrawer : AreaShapeFactoryDrawer
{
    protected override void DefaultExceptionGUI(SerializedProperty property)
    {
        switch (property.FindPropertyRelative("shape").GetEnumValue<AreaShapeFactory.Shape>())
        {
            case AreaShapeFactory.Shape.None:
                base.DefaultExceptionGUI(property);
                break;
            case AreaShapeFactory.Shape.Sectioned:
                SerializeSubFactories(property, "AdderArea:", "ReducerArea:");
                break;
            default:
                SerializeSubFactories(property, "AreaA:", "AreaB:");
                break;
        }
    }

    private void SerializeSubFactories(SerializedProperty property, string lable1, string lable2)
    {
        GUIStyle boldStyle = new GUIStyle(GUI.skin.label);
        boldStyle.fontStyle = FontStyle.Bold;

        EditorGUILayout.PropertyField(property.FindPropertyRelative("pivot"));

        GUILayout.Label(lable1, boldStyle);
        EditorGUILayout.PropertyField(property.FindPropertyRelative("subFactory1"));
        GUILayout.Space(8);
        GUILayout.Label(lable2, boldStyle);
        EditorGUILayout.PropertyField(property.FindPropertyRelative("subFactory2"));
    }
}
#endif
