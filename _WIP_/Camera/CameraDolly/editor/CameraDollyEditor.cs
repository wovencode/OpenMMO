//BY DX4D
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CameraDolly))]
[CanEditMultipleObjects]
public class CameraDollyEditor : Editor
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