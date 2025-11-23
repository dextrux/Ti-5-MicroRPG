using UnityEditor;
using UnityEngine;
using System;
using System.Linq;

[CustomPropertyDrawer(typeof(NaraMovementControllerReference))]
public class NaraMovementControllerReferenceDrawer : PropertyDrawer {
    private Type[] _controllerTypes;
    private string[] _controllerNames;

    private void LoadTypes() {
        if (_controllerTypes != null) return;

        _controllerTypes = TypeCache.GetTypesDerivedFrom<NaraMovementController>()
            .Where(t => !t.IsAbstract)
            .ToArray();

        _controllerNames = _controllerTypes
            .Select(t => t.Name)
            .ToArray();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        => EditorGUIUtility.singleLineHeight;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        LoadTypes();

        SerializedProperty typeNameProp = property.FindPropertyRelative("_typeName");
        Type currentType = string.IsNullOrEmpty(typeNameProp.stringValue)
            ? null
            : Type.GetType(typeNameProp.stringValue);

        int currentIndex = 0;

        if (currentType != null) {
            for (int i = 0; i < _controllerTypes.Length; i++) {
                if (_controllerTypes[i] == currentType) {
                    currentIndex = i;
                    break;
                }
            }
        }

        int newIndex = EditorGUI.Popup(position, label.text, currentIndex, _controllerNames);

        if (newIndex != currentIndex) {
            Type selected = _controllerTypes[newIndex];
            typeNameProp.stringValue = selected.AssemblyQualifiedName;
            property.serializedObject.ApplyModifiedProperties();
        }
    }
}
