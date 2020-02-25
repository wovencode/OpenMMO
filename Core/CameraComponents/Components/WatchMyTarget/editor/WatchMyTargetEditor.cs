//BY DX4D
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WatchMyTarget))]
[CanEditMultipleObjects]
public class WatchMyTargetEditor : Editor
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