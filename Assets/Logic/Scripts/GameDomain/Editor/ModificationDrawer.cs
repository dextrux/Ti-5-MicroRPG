using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Modification))]
public class ModificationDrawer : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {

        EditorGUI.BeginProperty(position, label, property);

        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        var typeRect = new Rect(position.x, position.y, position.width * 0.6f, position.height);
        var valueRect = new Rect(position.x + position.width * 0.65f, position.y, position.width * 0.35f, position.height);

        SerializedProperty typeProp = property.FindPropertyRelative("Type");
        SerializedProperty valueProp = property.FindPropertyRelative("Value");

        EditorGUI.PropertyField(typeRect, typeProp, GUIContent.none);
        EditorGUI.PropertyField(valueRect, valueProp, GUIContent.none);

        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}