#if UNITY_EDITOR

using UnityEditor;

#endif

public class IntShowConditionalAttribute : BaseShowConditionalAttribute
{
    public int[] conditionValues { get; private set; }

    public IntShowConditionalAttribute(string conditionFieldName, int conditionValue) : base(conditionFieldName)
    {
        conditionValues = new int[] { conditionValue };
    }

    public IntShowConditionalAttribute(string conditionFieldName, int[] conditionValues) : base(conditionFieldName)
    {
        this.conditionValues = conditionValues;
    }

#if UNITY_EDITOR

    public override bool GetShowResult(SerializedProperty sourcePropertyValue)
    {
        bool isShow = false;
        int comparingValue = 0;
        if (sourcePropertyValue != null)
        {
            switch (sourcePropertyValue.propertyType)
            {
                case SerializedPropertyType.Integer:
                    comparingValue = sourcePropertyValue.intValue;
                    break;
            }
        }
        foreach (int conditionValue in conditionValues)
        {
            if (comparingValue == conditionValue)
            {
                isShow = true;
                break;
            }
        }
        return isShow;
    }

#endif
}