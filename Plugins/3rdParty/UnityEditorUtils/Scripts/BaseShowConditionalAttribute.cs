using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

#endif

public abstract class BaseShowConditionalAttribute : PropertyAttribute
{
    public string conditionFieldName { get; private set; }

    public BaseShowConditionalAttribute(string conditionFieldName)
    {
        this.conditionFieldName = conditionFieldName;
    }

#if UNITY_EDITOR

    public abstract bool GetShowResult(SerializedProperty property);

#endif
}