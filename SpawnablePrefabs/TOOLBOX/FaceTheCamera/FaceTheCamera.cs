//BY: Davil [DX4D]
using UnityEngine;
using OpenMMO;

/// <summary>Turns this object to always face the camera.</summary>
public class FaceTheCamera : MonoBehaviour
{
    [Header("UPDATE FREQUENCY")]
    [Tooltip("How many FixedUpdate frames must pass before this component updates again?")]
    [SerializeField] [Range(1, 60)] int tickFrequency = 5; //TICK RATE

#if _CLIENT
    int frameCount = 0; //FRAME COUNTER
    void FixedUpdate()
    {
        //if (!PlayerComponent.localPlayer) return; //NOT LOGGED IN

        frameCount++; //INCREMENT TICK

        if (frameCount >= tickFrequency) //TICK THIS FRAME?
        {
            frameCount = 0; //RESET THE COUNTER

            if (Camera.main != null) gameObject.transform.forward = Camera.main.transform.forward; //FACE THE CAMERA
        }
    }
#endif
}