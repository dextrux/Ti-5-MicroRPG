#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.Rendering;

[CustomPropertyDrawer(typeof(AreaShapeFactory))]
public class AreaShapeFactoryDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty shapeProp = property.FindPropertyRelative("shape");

        EditorGUILayout.PropertyField(shapeProp);

        switch (shapeProp.GetEnumValue<AreaShapeFactory.Shape>())
        {
            case AreaShapeFactory.Shape.Circle:

                EditorGUILayout.PropertyField(property.FindPropertyRelative("externalRadius"));
                EditorGUILayout.PropertyField(property.FindPropertyRelative("pivot"));

                break;
            case AreaShapeFactory.Shape.Cone:

                EditorGUILayout.PropertyField(property.FindPropertyRelative("externalRadius"));
                EditorGUILayout.PropertyField(property.FindPropertyRelative("angle"));
                EditorGUILayout.PropertyField(property.FindPropertyRelative("pivot"));

                break;
            case AreaShapeFactory.Shape.Square:

                EditorGUILayout.PropertyField(property.FindPropertyRelative("height"));
                EditorGUILayout.PropertyField(property.FindPropertyRelative("width"));
                EditorGUILayout.PropertyField(property.FindPropertyRelative("pivot"));

                break;
            case AreaShapeFactory.Shape.Triagle:

                GUILayout.Label("Not Implemented");

                break;           
            default:
                DefaultExceptionGUI(property);
                break;
        }
    }

    protected virtual void DefaultExceptionGUI(SerializedProperty property)
    {
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(property.FindPropertyRelative("externalRadius"));
        EditorGUILayout.PropertyField(property.FindPropertyRelative("internalRadius"));
        EditorGUILayout.PropertyField(property.FindPropertyRelative("angle"));
        EditorGUILayout.PropertyField(property.FindPropertyRelative("height"));
        EditorGUILayout.PropertyField(property.FindPropertyRelative("width"));
        EditorGUILayout.PropertyField(property.FindPropertyRelative("pivot"));
    }
}
#endif
