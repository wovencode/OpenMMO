using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformHelpers
{
    public static void RemoveAllChildren(this Transform transform)
    {
        for (var i = transform.childCount - 1; i >= 0; --i)
        {
            var child = transform.GetChild(i);
            Object.Destroy(child.gameObject);
        }
    }

    public static void EnableComponentsInChildren<T>(this Transform transform, bool enabled) where T : Behaviour
    {
        var components = transform.GetComponentsInChildren<T>();
        for (var i = 0; i < components.Length; ++i)
        {
            var component = components[i];
            component.enabled = enabled;
        }
    }
}
