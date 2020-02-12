//BY: Davil [DX4D]
using UnityEngine;

/// <summary>Turns this object to always face the camera.</summary>
public class FaceTheCamera : MonoBehaviour
{
    [Tooltip("How many FixedUpdate frames must pass before this component updates again?")]
    [SerializeField] [Range(1, 60)] int tickFrequency = 5; //TICK RATE

    int frameCount = 0; //FRAME COUNTER
    void FixedUpdate()
    {
        frameCount++; //INCREMENT TICK

        if (frameCount >= tickFrequency) //TICK THIS FRAME?
        {
            frameCount = 0; //RESET THE COUNTER
            gameObject.transform.forward = Camera.main.transform.forward; //FACE THE CAMERA
        }
    }
}