//BY DX4D
using UnityEngine;

/// <summary>Turns this object to always face the camera.</summary>
public class FaceTheCamera : MonoBehaviour
{
    [Header("ORIENTATION")]
    [Tooltip("Always orient to the Vector3.up direction.")]
    [SerializeField] bool alwaysPointUp = true;

#pragma warning disable CS0414
    [Header("UPDATE FREQUENCY")]
    [Tooltip("How many update frames must pass before this component updates again?")]
    [SerializeField] [Range(1, 60)] int tickFrequency = 5; //TICK RATE
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

            if (Camera.main != null) gameObject.transform.forward = Camera.main.transform.forward; //FACE THE CAMERA
            if (alwaysPointUp) gameObject.transform.up = Vector3.up; //POINT UP
        }
    }
#endif
}