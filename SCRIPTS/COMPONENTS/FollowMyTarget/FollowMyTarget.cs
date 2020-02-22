//BY DX4D
//#define SMOOTH_DAMPING //Smooth Damping tracks the velocity and allows velocity to be limited.
using UnityEngine;
using OpenMMO;

/// <summary>Turns this object to always face the camera.</summary>
public class FollowMyTarget : MonoBehaviour
{
#pragma warning disable CS0414
    [Header("UPDATE FREQUENCY")]
    [Tooltip("How many update frames must pass before this component updates again?")]
    [SerializeField] [Range(1, 60)] int tickFrequency = 1; //TICK RATE

    [Header("TARGET")]
    [SerializeField] Transform target;

    [Header("FOLLOW SPEED")]
    [SerializeField] float followSpeed = 0.15f;

    [Header("FOLLOW OFFSET")]
    [SerializeField] int heightOffset = 6;
    [SerializeField] int followOffset = 12;

#if SMOOTH_DAMPING
    [Header("VELOCITY")]
    [SerializeField] Vector3 velocity = Vector3.zero;
    [SerializeField] float maxSpeed = 100.0f;
#endif

#pragma warning restore CS0414

#if _CLIENT
    public Vector3 offset { get { return new Vector3(0, heightOffset, -followOffset); } }

    int frameCount = 0; //FRAME COUNTER
    void FixedUpdate()
    {
        frameCount++; //INCREMENT TICK

        if (frameCount >= tickFrequency) //TICK THIS FRAME?
        {
            frameCount = 0; //RESET THE COUNTER
            if (!target)
            {
                GameObject player = PlayerComponent.localPlayer;
                if (player) target = player.transform;
            }
            if (target)
            {   
#if SMOOTH_DAMPING
                transform.position = Vector3.SmoothDamp(transform.position, target.position + offset, ref velocity, Time.deltaTime * followSpeed, maxSpeed);
                //print(velocity);
#else
                transform.position = Vector3.Lerp(transform.position, target.position + offset, followSpeed * Time.deltaTime);
#endif
            }
        }
    }
#endif
}