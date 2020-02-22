//BY DX4D
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SeasonalTileUpdater))]
[CanEditMultipleObjects]
public class SeasonalTileUpdaterEditor : Editor
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