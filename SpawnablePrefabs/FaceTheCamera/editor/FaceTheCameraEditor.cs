//BY DX4D
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FaceTheCamera))]
[CanEditMultipleObjects]
public class FaceTheCameraEditor : Editor
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