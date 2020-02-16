//BY DX4D
using UnityEngine;

/// <summary>This object will always point up.</summary>
public class AlwaysPointUp : MonoBehaviour
{
#pragma warning disable CS0414
    [Header("UPDATE FREQUENCY")]
    [Tooltip("How many update frames must pass before this component updates again?")]
    [SerializeField] [Range(1, 60)] int tickFrequency = 60; //TICK RATE
#pragma warning restore CS0414

#if _CLIENT
    int frameCount = 0; //FRAME COUNTER
    void LateUpdate()
    {
        //if (!PlayerComponent.localPlayer) return; //NOT LOGGED IN

        frameCount++; //INCREMENT TICK

        if (frameCount >= tickFrequency) //TICK THIS FRAME?
        {
            frameCount = 0; //RESET THE COUNTER

            gameObject.transform.up = Vector3.up; //POINT UP
        }
    }
#endif
}