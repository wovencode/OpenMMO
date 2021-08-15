//BY DX4D
using System;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class HookAttribute : DevExtMethodsAttribute
{
    //INHERITED METHODS
    //public string BaseMethodName { get; private set; }

    public HookAttribute(string baseMethodName) : base(baseMethodName) { }
}

/// <summary>
/// Hooks allow you to link methods so they trigger together.
///
///EXAMPLE - HOOKING OnValidate()
///private void OnValidate()
///{
///    Hook.HookMethod(this, nameof(OnValidate)); //HOOK USAGE: [Hook("OnValidate")] void MyMethod() { }
///}
/// </summary>
public static class Hook
{
    /// <summary>Call this from inside the method you are hooking into.</summary>
    /// <typeparam name="T">(optional)</typeparam> <param name="obj"></param> <param name="baseMethodName"></param> <param name="args"></param>
    public static void HookMethod<T>(this T obj, string baseMethodName, params object[] args)
    {
        string header = string.Empty;
        string spacer = " - ";
#if _CLIENT && _SERVER
        header += "<b>[<color=purple>" + "HOST" + "</color>]" + spacer + "[HOOKS]</b>";
#elif _SERVER
        header += "[SERVER]" + spacer + "[HOOKS]";
#elif _CLIENT
        header += "<b>[<color=orange>" + "CLIENT" + "</color>]" + spacer + "[HOOKS]</b>";
#endif
        UnityEngine.Debug.Log(header + spacer
            + "Hooking into method " + baseMethodName + "...");

        DevExtUtils.InvokeInstanceDevExtMethods(obj, baseMethodName, args);
    }
}