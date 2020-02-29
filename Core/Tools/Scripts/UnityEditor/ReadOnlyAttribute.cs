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
	/// Used to create a inspector exposable property field that cannot be modified by the user (= read only).
	/// </summary>
	[AttributeUsage (AttributeTargets.Field,Inherited = true)]
	public class ReadOnlyAttribute : PropertyAttribute {}
	
	/// <summary>
	/// Renders the read-only property drawer
	/// </summary>
#if UNITY_EDITOR
	[UnityEditor.CustomPropertyDrawer (typeof(ReadOnlyAttribute))]
	public class ReadOnlyAttributeDrawer : UnityEditor.PropertyDrawer
	{
		public override void OnGUI(Rect rect, UnityEditor.SerializedProperty prop, GUIContent label)
		{
			bool wasEnabled = GUI.enabled;
			GUI.enabled = false;
			UnityEditor.EditorGUI.PropertyField(rect,prop);
			GUI.enabled = wasEnabled;
		}
	}
#endif
	
}