#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.Rendering;

[CustomPropertyDrawer(typeof(SkillEffectFactory))]
public class SkillEffectFactoryDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty effectProp = property.FindPropertyRelative("effect");

        EditorGUILayout.PropertyField(effectProp);

        switch (effectProp.GetEnumValue<SkillEffectFactory.Effect>())
        {
            case SkillEffectFactory.Effect.Damage:

                EditorGUILayout.PropertyField(property.FindPropertyRelative("intValue"), new GUIContent("Damage"));
                break;
            case SkillEffectFactory.Effect.Heal:

                EditorGUILayout.PropertyField(property.FindPropertyRelative("intValue"), new GUIContent("Heal"));

                break;
            default:

                EditorGUILayout.PropertyField(property.FindPropertyRelative("intValue"));

                break;
        }
    }
}
#endif
