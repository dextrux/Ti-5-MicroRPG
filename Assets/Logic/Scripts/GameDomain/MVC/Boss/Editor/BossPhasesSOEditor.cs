using UnityEditor;
using UnityEngine;

namespace Logic.Scripts.GameDomain.MVC.Boss
{
    [CustomEditor(typeof(BossPhasesSO))]
    public class BossPhasesSOEditor : Editor
    {
        private SerializedProperty _phasesProp;

        private void OnEnable()
        {
            _phasesProp = serializedObject.FindProperty("_phases");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            if (_phasesProp != null)
            {
                EditorGUILayout.LabelField("Phases", EditorStyles.boldLabel);
                for (int i = 0; i < _phasesProp.arraySize; i++)
                {
                    SerializedProperty elem = _phasesProp.GetArrayElementAtIndex(i);
                    EditorGUILayout.BeginVertical(GUI.skin.box);

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField($"Phase {i}", EditorStyles.boldLabel);
                    if (GUILayout.Button("Remove", GUILayout.Width(70)))
                    {
                        _phasesProp.DeleteArrayElementAtIndex(i);
                        break;
                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.PropertyField(elem.FindPropertyRelative("Name"));
                    SerializedProperty triggerType = elem.FindPropertyRelative("TriggerType");
                    EditorGUILayout.PropertyField(triggerType);
                    BossPhasesSO.PhaseTriggerType mode = (BossPhasesSO.PhaseTriggerType)triggerType.enumValueIndex;
                    if (mode == BossPhasesSO.PhaseTriggerType.HealthPercentBelow)
                    {
                        EditorGUILayout.PropertyField(elem.FindPropertyRelative("HealthPercentThreshold"), new GUIContent("Percent Threshold (0-1)"));
                    }
                    else
                    {
                        EditorGUILayout.PropertyField(elem.FindPropertyRelative("HealthAbsoluteThreshold"), new GUIContent("Flat HP Threshold"));
                    }
                    EditorGUILayout.PropertyField(elem.FindPropertyRelative("Behavior"));
                    EditorGUILayout.EndVertical();
                }
            }

            if (GUILayout.Button("Add Phase"))
            {
                int idx = _phasesProp != null ? _phasesProp.arraySize : 0;
                if (_phasesProp != null)
                {
                    _phasesProp.InsertArrayElementAtIndex(idx);
                }
            }

            using (new EditorGUI.DisabledScope(_phasesProp == null || _phasesProp.arraySize == 0))
            {
                if (GUILayout.Button("Clear All Phases"))
                {
                    if (_phasesProp != null)
                    {
                        _phasesProp.ClearArray();
                    }
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}

