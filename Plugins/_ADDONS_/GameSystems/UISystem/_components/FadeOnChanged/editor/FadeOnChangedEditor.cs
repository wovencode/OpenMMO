//BY DX4D
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FadeOnChanged))]
[CanEditMultipleObjects]
public class FadeOnChangedEditor : Editor
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