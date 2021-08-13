//BY DX4D
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerNameplateUpdater))]
[CanEditMultipleObjects]
public class NameplateUpdaterEditor : Editor
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