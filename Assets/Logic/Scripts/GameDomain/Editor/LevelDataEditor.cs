using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelData), true)]
public class LevelDataEditor : Editor {
    public override void OnInspectorGUI() {
        serializedObject.Update();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("levelAddress"));

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Movement Controller", EditorStyles.boldLabel);

        EditorGUILayout.PropertyField(serializedObject.FindProperty("controller"));

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Level Specific Settings", EditorStyles.boldLabel);

        DrawRemainingProperties();

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawRemainingProperties() {
        SerializedProperty iterator = serializedObject.GetIterator();
        iterator.NextVisible(true);

        while (iterator.NextVisible(false)) {
            if (iterator.name == "levelAddress") continue;
            if (iterator.name == "controller") continue;

            EditorGUILayout.PropertyField(iterator, true);
        }
    }
}
