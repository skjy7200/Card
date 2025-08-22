// Assets/Editor/EffectDrawer.cs
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(Effect))]
public class EffectDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        bool showStatus = property.FindPropertyRelative("effectType").enumValueIndex == (int)EffectType.StatusEffect;
        int lines = showStatus ? 5 : 4;
        return lines * EditorGUIUtility.singleLineHeight + 4f;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        float lineHeight = EditorGUIUtility.singleLineHeight;
        float padding = 2f;
        Rect r = new Rect(position.x, position.y, position.width, lineHeight);

        SerializedProperty effectType = property.FindPropertyRelative("effectType");
        SerializedProperty value = property.FindPropertyRelative("value");
        SerializedProperty duration = property.FindPropertyRelative("duration");
        SerializedProperty description = property.FindPropertyRelative("description");
        SerializedProperty statusEffect = property.FindPropertyRelative("statusEffect");

        EditorGUI.PropertyField(r, effectType);
        r.y += lineHeight + padding;
        EditorGUI.PropertyField(r, value);
        r.y += lineHeight + padding;
        EditorGUI.PropertyField(r, duration);
        r.y += lineHeight + padding;
        EditorGUI.PropertyField(r, description);
        r.y += lineHeight + padding;

        if ((EffectType)effectType.enumValueIndex == EffectType.StatusEffect)
        {
            EditorGUI.PropertyField(r, statusEffect);
        }

        EditorGUI.EndProperty();
    }
}
