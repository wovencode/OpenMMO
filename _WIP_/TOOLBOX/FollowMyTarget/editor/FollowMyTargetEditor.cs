//BY DX4D
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FollowMyTarget))]
[CanEditMultipleObjects]
public class FollowMyTargetEditor : Editor
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