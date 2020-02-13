//BY: Davil [DX4D]
using UnityEngine;
using OpenMMO;

/// <summary>Turns this object to always face a target.</summary>
public class WatchMyTarget : MonoBehaviour
{
    [Header("UPDATE FREQUENCY")]
    [Tooltip("How many update frames must pass before this component updates again?")]
    [SerializeField] [Range(1, 60)] int tickFrequency = 1; //TICK RATE

    [Header("TARGET")]
    [SerializeField] Transform target;

    [Header("LOOK SPEED")]
    [SerializeField] float lookSpeed = 0.15f;

    [Header("FOLLOW OFFSET")]
    [SerializeField] int heightOffset = 1;
    [SerializeField] int zoomOffset = -3;
    
    public Vector3 offset { get { return new Vector3(0, heightOffset, zoomOffset); } }

#if _CLIENT
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
                transform.LookAt( Vector3.Lerp(transform.position, target.position + offset, lookSpeed * Time.deltaTime) );
            }
        }
    }
#endif
}