//BY DX4D
using UnityEngine;
using Mirror;

public class DisableTimer : MonoBehaviour
{
    [Header("DISABLE IN:")]
    [Tooltip("The amount of time (in seconds) before the attached gameobject is deactivated.")]
    [SerializeField] double duration = 24.0f;
    double endTime;

    void Awake()
    {
        endTime = NetworkTime.time + duration;//Time.fixedTime
    }
    
    void Update()
    {
        if (endTime <= 0 && gameObject.activeSelf) { endTime = NetworkTime.time + duration; }
        else if (endTime > 0 && NetworkTime.time > endTime) { endTime = 0; gameObject.SetActive(false); }
    }
}
