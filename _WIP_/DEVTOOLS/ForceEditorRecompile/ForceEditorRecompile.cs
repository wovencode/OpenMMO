using UnityEditor;

public partial class Project
{
    public const string defineSymbol = "forceeditorrecompile";
    public static void ForceEditorRecompile()
    {
        BuildTargetGroup target = GetBuildTargetGroup();
        string rawSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(target);

        PlayerSettings.SetScriptingDefineSymbolsForGroup(target, rawSymbols + defineSymbol);
        PlayerSettings.SetScriptingDefineSymbolsForGroup(target, rawSymbols);
        /*if (rawSymbols.Contains(defineSymbol))
        {
            PlayerSettings.SetScriptingDefineSymbolsForGroup(target, rawSymbols.TrimEnd(defineSymbol.ToCharArray()));
        }
        else
        {
            PlayerSettings.SetScriptingDefineSymbolsForGroup(target, rawSymbols + defineSymbol);
        }*/
    }

    public static BuildTargetGroup GetBuildTargetGroup()
    {
        return BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget);
    }
}