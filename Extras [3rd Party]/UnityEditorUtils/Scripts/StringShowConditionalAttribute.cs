#if UNITY_EDITOR

using UnityEditor;

#endif

public class StringShowConditionalAttribute : BaseShowConditionalAttribute
{
    public string[] conditionValues { get; private set; }

    public StringShowConditionalAttribute(string conditionFieldName, string conditionValue) : base(conditionFieldName)
    {
        conditionValues = new string[] { conditionValue };
    }

    public StringShowConditionalAttribute(string conditionFieldName, string[] conditionValues) : base(conditionFieldName)
    {
        this.conditionValues = conditionValues;
    }

#if UNITY_EDITOR

    public override bool GetShowResult(SerializedProperty sourcePropertyValue)
    {
        bool isShow = false;
        string comparingValue = "";
        if (sourcePropertyValue != null)
        {
            switch (sourcePropertyValue.propertyType)
            {
                case SerializedPropertyType.Enum:
                    comparingValue = sourcePropertyValue.enumNames[sourcePropertyValue.enumValueIndex];
                    break;

                case SerializedPropertyType.String:
                    comparingValue = sourcePropertyValue.stringValue;
                    break;
            }
        }
        if (!string.IsNullOrEmpty(comparingValue))
        {
            foreach (string conditionValue in conditionValues)
            {
                if (comparingValue.Equals(conditionValue))
                {
                    isShow = true;
                    break;
                }
            }
        }
        return isShow;
    }

#endif
}