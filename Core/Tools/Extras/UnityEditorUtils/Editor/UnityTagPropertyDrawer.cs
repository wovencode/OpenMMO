using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(UnityTag))]
public class UnityTagPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
    {
        EditorGUI.BeginProperty(_position, GUIContent.none, _property);
        SerializedProperty tag = _property.FindPropertyRelative("tag");
        _position = EditorGUI.PrefixLabel(_position, GUIUtility.GetControlID(FocusType.Passive), _label);
        if (tag != null)
        {
            tag.stringValue = EditorGUI.TagField(_position, tag.stringValue);
        }
        EditorGUI.EndProperty();
    }
}