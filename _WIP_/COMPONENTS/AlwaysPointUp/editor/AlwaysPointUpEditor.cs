//BY DX4D
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AlwaysPointUp))]
[CanEditMultipleObjects]
public class AlwaysPointUpEditor : Editor
{
    public override void OnInspectorGUI()
    {
        GUILayout.BeginVertical("box");
        {
            base.OnInspectorGUI();
        }
        GUILayout.EndVertical();
    }
}