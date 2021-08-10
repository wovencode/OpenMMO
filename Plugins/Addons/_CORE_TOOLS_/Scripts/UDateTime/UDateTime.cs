//by Fhiz
using OpenMMO;
using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace OpenMMO
{

	/// <summary>
	/// We have to use UDateTime instead of DateTime on our classes, we still typically need to either cast this to a DateTime or read the DateTime field directly.
	/// </summary>
	[System.Serializable]
	public partial class UDateTime : ISerializationCallbackReceiver {
		[HideInInspector] public DateTime dateTime;

		// if you don't want to use the PropertyDrawer then remove HideInInspector here
		[HideInInspector] [SerializeField] private string _dateTime;

		public static implicit operator DateTime(UDateTime udt) {
			return (udt.dateTime);
		}

		public static implicit operator UDateTime(DateTime dt) {
			return new UDateTime() {dateTime = dt};
		}

		public void OnAfterDeserialize() {
			DateTime.TryParse(_dateTime, out dateTime);
		}

		public void OnBeforeSerialize() {
			_dateTime = dateTime.ToString();
		}
	}
	
	/// <summary>
	/// If we implement this PropertyDrawer then we keep the label next to the text field.
	/// </summary>
	#if UNITY_EDITOR
	[CustomPropertyDrawer(typeof(UDateTime))]
	public partial class UDateTimeDrawer : PropertyDrawer {
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			EditorGUI.BeginProperty(position, label, property);
			position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
			var indent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;
			Rect amountRect = new Rect(position.x, position.y, position.width, position.height);
			EditorGUI.PropertyField(amountRect, property.FindPropertyRelative("_dateTime"), GUIContent.none);
			EditorGUI.indentLevel = indent;
			EditorGUI.EndProperty();
		}
	}
	#endif
	
}