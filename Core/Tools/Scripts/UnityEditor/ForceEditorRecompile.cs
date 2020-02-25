#if UNITY_EDITOR
using UnityEditor;

public partial class Project
{
    public const string defineSymbol = "recompiling";

    /// <summary>Force a recompile in the editor.
    /// NOTE: This will trigger a rebuild twice...no way around it or it becomes unreliable. It works for now until Unity fixes the AssetDatabase.Refresh function.</summary>
    public static void ForceEditorRecompile()
    {
        BuildTargetGroup target = GetBuildTargetGroup();
        string rawSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(target);

        PlayerSettings.SetScriptingDefineSymbolsForGroup(target, rawSymbols + defineSymbol);
        PlayerSettings.SetScriptingDefineSymbolsForGroup(target, rawSymbols);
    }

    public static BuildTargetGroup GetBuildTargetGroup()
    {
        return BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget);
    }
}
#endif