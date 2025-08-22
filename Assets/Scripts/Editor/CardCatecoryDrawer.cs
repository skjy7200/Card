using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(CardCategory))]
public class CardCategoryDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        property.intValue = (int)(CardCategory)EditorGUI.EnumFlagsField(position, label, (CardCategory)property.intValue);
    }
}
