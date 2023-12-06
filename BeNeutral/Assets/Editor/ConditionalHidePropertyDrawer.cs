using UnityEditor;
using UnityEngine;
using Attributes;

[CustomPropertyDrawer(typeof(ConditionalHideAttribute), true)]
public class ConditionalHidePropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Get the attribute
        ConditionalHideAttribute conditionalHideAttribute = (ConditionalHideAttribute)attribute;

        // Check if the condition is met
        bool enabled = GetConditionalHideAttributeResult(conditionalHideAttribute, property);

        // Disable the property if the condition is not met
        using (var disabledGroup = new EditorGUI.DisabledGroupScope(!enabled))
        {
            EditorGUI.PropertyField(position, property, label, true);
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // Get the attribute
        ConditionalHideAttribute conditionalHideAttribute = (ConditionalHideAttribute)attribute;

        // Check if the condition is met
        bool enabled = GetConditionalHideAttributeResult(conditionalHideAttribute, property);

        // If the condition is not met, hide the property
        if (!enabled)
        {
            return 0f;
        }

        // Otherwise, use the default property height
        return EditorGUI.GetPropertyHeight(property, label);
    }

    private bool GetConditionalHideAttributeResult(ConditionalHideAttribute attribute, SerializedProperty property)
    {
        // Get the condition property
        SerializedProperty conditionProperty = property.serializedObject.FindProperty(attribute.conditionName);

        // Check if the condition property exists and its boolean value
        if (conditionProperty != null)
        {
            return conditionProperty.boolValue == attribute.showIfTrue;
        }

        // Default to true if the condition property is not found
        return true;
    }
}