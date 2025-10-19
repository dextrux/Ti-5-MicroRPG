using UnityEditor;
using UnityEngine;

namespace Logic.Scripts.GameDomain.MVC.Boss.Editor
{
    [CustomEditor(typeof(BossAttack))]
    public class BossAttackEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            SerializedProperty attackTypeProp = serializedObject.FindProperty("_attackType");
            SerializedProperty proteanProp = serializedObject.FindProperty("_protean");
            SerializedProperty featherProp = serializedObject.FindProperty("_feather");
            SerializedProperty effectsProp = serializedObject.FindProperty("_effects");

            EditorGUILayout.PropertyField(attackTypeProp);

            int attackTypeIdx = attackTypeProp.enumValueIndex;
            if (attackTypeIdx == 0)
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Protean Cones", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(proteanProp, true);
            }
            else if (attackTypeIdx == 1)
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Feather Lines", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(featherProp, true);
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Effects", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(effectsProp, true);

            serializedObject.ApplyModifiedProperties();
        }
    }
}


