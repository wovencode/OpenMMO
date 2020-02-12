
using UnityEngine;
using System.Collections.Generic;

[DisallowMultipleComponent]
public class DontDestroyOnLoad : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
