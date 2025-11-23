using UnityEditor;
using UnityEngine;

namespace Logic.Scripts.GameDomain.MVC.Boss
{
    [CustomEditor(typeof(BossAttack))]
    public class BossAttackEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            SerializedProperty effects = serializedObject.FindProperty("_effects");
            SerializedProperty attackType = serializedObject.FindProperty("_attackType");
			SerializedProperty displacementPriority = serializedObject.FindProperty("_displacementPriority");
            SerializedProperty protean = serializedObject.FindProperty("_protean");
            SerializedProperty feather = serializedObject.FindProperty("_feather");
            SerializedProperty wingSlash = serializedObject.FindProperty("_wingSlash");
            SerializedProperty orb = serializedObject.FindProperty("_orb");
            SerializedProperty featherIsPull = serializedObject.FindProperty("_featherIsPull");
			SerializedProperty skySwords = serializedObject.FindProperty("_skySwords");
			SerializedProperty skySwordsIsPull = serializedObject.FindProperty("_skySwordsIsPull");

            EditorGUILayout.PropertyField(effects, true);
            EditorGUILayout.PropertyField(attackType);
			EditorGUILayout.PropertyField(displacementPriority, new GUIContent("Displacement Priority"));

			// 0 = ProteanCones, 1 = FeatherLines, 2 = WingSlash, 3 = Orb, 4 = HookAwakening, 5 = SkySwords
            switch (attackType.enumValueIndex)
            {
                case 0: // ProteanCones
                    EditorGUILayout.PropertyField(protean, true);
                    break;
                case 1: // FeatherLines
                    EditorGUILayout.PropertyField(feather, true);
                    EditorGUILayout.PropertyField(featherIsPull, new GUIContent("Feather Is Pull"));
                    break;
                case 2: // WingSlash
                    EditorGUILayout.PropertyField(wingSlash, true);
                    break;
                case 3: // Orb
                    EditorGUILayout.PropertyField(orb, true);
                    break;
				case 5: // SkySwords
					EditorGUILayout.PropertyField(skySwords, true);
					EditorGUILayout.PropertyField(skySwordsIsPull, new GUIContent("SkySwords Is Pull"));
					break;
                default:
                    break;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
