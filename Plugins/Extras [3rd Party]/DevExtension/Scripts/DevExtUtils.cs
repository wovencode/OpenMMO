using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public static class DevExtUtils
{
    private static readonly Dictionary<Type, Dictionary<string, MethodInfo[]>> cacheDevExtMethods = new Dictionary<Type, Dictionary<string, MethodInfo[]>>();

    // Optimizing garbage collection
    private static MethodInfo[] tempMethods;

    private static int tempLoopCounter;
    private static DevExtMethodsAttribute tempAttribute;
    private const BindingFlags InstanceMethodBindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
    private const BindingFlags StaticMethodBindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

    /// <summary>
    /// This will calls all methods from `obj` that have attributes "DevExtMethodsAttribute("`baseMethodName`")" with any number of arguments that can be set via `args`
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <param name="baseMethodName"></param>
    /// <param name="args"></param>
    public static void InvokeInstanceDevExtMethods<T>(this T obj, string baseMethodName, params object[] args)
    {
        if (obj == null)
            return;
        InvokeDevExtMethods(obj.GetType(), obj, baseMethodName, InstanceMethodBindingFlags, args);
    }

    public static T InvokeInstanceDevExtMethodsLoopItself<T>(this T obj, string baseMethodName, params object[] args)
    {
        if (obj == null)
            return default(T);
        return InvokeDevExtMethodsLoopItself<T>(obj.GetType(), obj, baseMethodName, InstanceMethodBindingFlags, args);
    }

    /// <summary>
    /// This will calls all static methods which its type is `type` that have attributes "DevExtMethodsAttribute("`baseMethodName`")" with any number of arguments that can be set via `args`
    /// </summary>
    /// <param name="type"></param>
    /// <param name="baseMethodName"></param>
    /// <param name="args"></param>
    public static void InvokeStaticDevExtMethods(Type type, string baseMethodName, params object[] args)
    {
        InvokeDevExtMethods(type, null, baseMethodName, StaticMethodBindingFlags, args);
    }

    private static void InvokeDevExtMethods(Type type, object obj, string baseMethodName, BindingFlags bindingFlags, params object[] args)
    {

        try
        {
            if (!cacheDevExtMethods.ContainsKey(type) || !cacheDevExtMethods[type].ContainsKey(baseMethodName))
            {
                if (!cacheDevExtMethods.ContainsKey(type))
                    cacheDevExtMethods.Add(type, new Dictionary<string, MethodInfo[]>());
                cacheDevExtMethods[type].Add(baseMethodName, null);
                tempMethods = type.GetMethods(bindingFlags).Where(a =>
                {
                    tempAttribute = (DevExtMethodsAttribute)a.GetCustomAttribute(typeof(DevExtMethodsAttribute), true);
                    return tempAttribute != null && tempAttribute.BaseMethodName.Equals(baseMethodName);
                }).ToArray();
                if (tempMethods != null && tempMethods.Length > 0)
                    cacheDevExtMethods[type][baseMethodName] = tempMethods;
            }
            if (!cacheDevExtMethods[type].TryGetValue(baseMethodName, out tempMethods) || tempMethods == null || tempMethods.Length == 0)
                return;  
            for (tempLoopCounter = 0; tempLoopCounter < tempMethods.Length; ++tempLoopCounter)
            {
                tempMethods[tempLoopCounter].Invoke(obj, args);
            }
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    private static T InvokeDevExtMethodsLoopItself<T>(Type type, object obj, string baseMethodName, BindingFlags bindingFlags, params object[] args)
    {
        try
        {
            if (!cacheDevExtMethods.ContainsKey(type) || !cacheDevExtMethods[type].ContainsKey(baseMethodName))
            {
                if (!cacheDevExtMethods.ContainsKey(type))
                    cacheDevExtMethods.Add(type, new Dictionary<string, MethodInfo[]>());
                cacheDevExtMethods[type].Add(baseMethodName, null);
                tempMethods = type.GetMethods(bindingFlags).Where(a =>
                {
                    tempAttribute = (DevExtMethodsAttribute)a.GetCustomAttribute(typeof(DevExtMethodsAttribute), true);
                    return tempAttribute != null && tempAttribute.BaseMethodName.Equals(baseMethodName);
                }).ToArray();
                if (tempMethods != null && tempMethods.Length > 0)
                    cacheDevExtMethods[type][baseMethodName] = tempMethods;
            }
            if (!cacheDevExtMethods[type].TryGetValue(baseMethodName, out tempMethods) || tempMethods == null || tempMethods.Length == 0)
                return (T)obj;
            for (tempLoopCounter = 0; tempLoopCounter < tempMethods.Length; ++tempLoopCounter)
            {
                obj = tempMethods[tempLoopCounter].Invoke(obj, args);
            }
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
        return (T)obj;
    }
}