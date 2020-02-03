// =======================================================================================
// UDateTime
// by Weaver (Fhiz)
// MIT licensed
// =======================================================================================

using OpenMMO;
using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace OpenMMO
{
	
	// -----------------------------------------------------------------------------------
	// we have to use UDateTime instead of DateTime on our classes
	// we still typically need to either cast this to a DateTime or read the DateTime field directly
	// -----------------------------------------------------------------------------------
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
	
	// -----------------------------------------------------------------------------------
	// If we implement this PropertyDrawer then we keep the label next to the text field
	// -----------------------------------------------------------------------------------
	#if UNITY_EDITOR
	[CustomPropertyDrawer(typeof(UDateTime))]
	public partial class UDateTimeDrawer : PropertyDrawer {
		// Draw the property inside the given rect
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			// Using BeginProperty / EndProperty on the parent property means that
			// prefab override logic works on the entire property.
			EditorGUI.BeginProperty(position, label, property);

			// Draw label
			position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

			// Don't make child fields be indented
			var indent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;

			// Calculate rects
			Rect amountRect = new Rect(position.x, position.y, position.width, position.height);

			// Draw fields - passs GUIContent.none to each so they are drawn without labels
			EditorGUI.PropertyField(amountRect, property.FindPropertyRelative("_dateTime"), GUIContent.none);

			// Set indent back to what it was
			EditorGUI.indentLevel = indent;

			EditorGUI.EndProperty();
		}
	}
	#endif
	
	// -----------------------------------------------------------------------------------
	
}

// =======================================================================================