#if UNITY_EDITOR

using UnityEditor;

#endif

public class BoolShowConditionalAttribute : BaseShowConditionalAttribute
{
    public bool conditionValue { get; private set; }

    public BoolShowConditionalAttribute(string conditionFieldName, bool conditionValue) : base(conditionFieldName)
    {
        this.conditionValue = conditionValue;
    }

#if UNITY_EDITOR

    public override bool GetShowResult(SerializedProperty sourcePropertyValue)
    {
        bool isShow = false;
        if (sourcePropertyValue != null)
        {
            switch (sourcePropertyValue.propertyType)
            {
                case SerializedPropertyType.Boolean:
                    isShow = sourcePropertyValue.boolValue == conditionValue;
                    break;
            }
        }
        return isShow;
    }

#endif
}