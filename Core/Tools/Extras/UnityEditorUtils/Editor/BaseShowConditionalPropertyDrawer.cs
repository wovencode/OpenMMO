using UnityEditor;
using UnityEngine;

public abstract class BaseShowConditionalPropertyDrawer<T> : PropertyDrawer where T : BaseShowConditionalAttribute
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        T condAttribute = (T)attribute;
        bool isShow = GetShowResult(condAttribute, property);
        if (isShow)
            return EditorGUI.GetPropertyHeight(property, label);
        else
            return -EditorGUIUtility.standardVerticalSpacing;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        T condAttribute = (T)attribute;
        bool isShow = GetShowResult(condAttribute, property);
        if (isShow)
            EditorGUI.PropertyField(position, property, label, true);
    }

    private bool GetShowResult(T attribute, SerializedProperty property)
    {
        var propertyPath = property.propertyPath;
        var conditionPath = propertyPath.Replace(property.name, attribute.conditionFieldName);
        var sourcePropertyValue = property.serializedObject.FindProperty(conditionPath);
#if UNITY_EDITOR
        return attribute.GetShowResult(sourcePropertyValue);
#else
        return false;
#endif
    }
}