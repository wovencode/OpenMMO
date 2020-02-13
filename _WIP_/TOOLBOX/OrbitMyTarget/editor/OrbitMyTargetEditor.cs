//BY DX4D
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(OrbitMyTarget))]
[CanEditMultipleObjects]
public class OrbitMyTargetEditor : Editor
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