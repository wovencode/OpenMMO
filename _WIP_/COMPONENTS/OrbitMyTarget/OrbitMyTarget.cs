//BY DX4D
using System;
using UnityEngine;
using OpenMMO;

/// <summary>Turns this object to face the same direction of the target.</summary>
public class OrbitMyTarget : MonoBehaviour
{
#pragma warning disable CS0414
    [Header("UPDATE FREQUENCY")]
    [Tooltip("How many update frames must pass before this component updates again?")]
    [SerializeField] [Range(1, 60)] int tickFrequency = 1; //TICK RATE

    [Header("TARGET")]
    [SerializeField] Transform target;

    [Header("INPUT")]
    [SerializeField] KeyCode orbitKey = KeyCode.Mouse1;
    [SerializeField] string zoomAxis = "Mouse ScrollWheel";
    [SerializeField] string horizontalAxis = "Mouse X";
    [SerializeField] string verticalAxis = "Mouse Y";

    [Header("MOVEMENT SPEED")]
    //[SerializeField] float followSpeed = 10f; //DEPRRECIATED - unused
    [SerializeField] float turnSpeed = 2f;//0.02f;
    [SerializeField] float pitchSpeed = 7f;//0.06f;
    [SerializeField] float zoomSpeed = 5f;//0.06f;

    [Header("ZOOM")]
    public float distance = 5.0f;
    public float distanceMin = .5f;
    public float distanceMax = 25f;

     [Header("OFFSETS")]
    [SerializeField] float heightOffset = 3;

     [Header("ATTACH TO TARGET")]
    [SerializeField] bool attachToTarget = false;
    //[SerializeField] int zoomOffset = 12;

    public Vector3 offset { get { return new Vector3(0f, heightOffset, -distance); } }

    //public new Rigidbody rigidbody = null;

    //public float horizontalSpeed = 2.4f;//120.0f;
    //public float verticalSpeed = 7.2f;//120.0f;

     //CONSTRAINTS
     [Header("CONSTRAINTS")]
    [Tooltip("0 = disabled")]
    public float horizontalMax = 0f; //0 = disabled
    public float horizontalMin = -360f;

    public float verticalMin = 1f;
    public float verticalMax = 80f;
#pragma warning restore CS0414

#if _CLIENT
    float x = 0.0f;
    float y = 0.0f;
    
    //UPDATE
    bool set = false;
    bool attached = false;
    void Update()
    {
        if (set) return;

        if (!target)
        {
            GameObject player = PlayerComponent.localPlayer;
            if (player) target = player.transform;
        }
        if (target)
        {
            set = true;
            Vector3 angles = target.transform.eulerAngles;
            x = angles.y;
            y = angles.x;

            transform.forward = target.forward;

            if (attachToTarget && !attached)
            {
                attached = true;
                gameObject.transform.SetParent(target, false);
            }
        }
        //if (!rigidbody) rigidbody = GetComponent<Rigidbody>();

        // Make the rigid body not change rotation
        //if (rigidbody != null)
        //{
        //    rigidbody.freezeRotation = true;
        //}
    }

    //FIXED UPDATE
    int frameCount = 0; //FRAME COUNTER
    void LateUpdate()
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
                //if (Input.GetKeyDown(orbitKey)) { transform.position = target.position + new Vector3(0f, heightOffset, -distance); } 
                if (Input.GetKey(orbitKey))
                {
                    x += Input.GetAxis(horizontalAxis) * turnSpeed * distance;
                    y -= Input.GetAxis(verticalAxis) * pitchSpeed;

                    if (horizontalMax > 0) x = ClampAngle(x, horizontalMin, horizontalMax);
                    if (verticalMax > 0) y = ClampAngle(y, verticalMin, verticalMax);
                }

                //Vector3 angle = target.transform.eulerAngles;
                Quaternion rotation = Quaternion.Euler(y, x, 0);

                distance = Mathf.Clamp(distance - Input.GetAxis(zoomAxis) * zoomSpeed, distanceMin, distanceMax);

                //RaycastHit hit;
                //if (Physics.Linecast(target.position, transform.position, out hit))
                //{
                //    distance -= hit.distance;
                //}

                Vector3 invertedDistance = new Vector3(0f, 0f, -distance);
                Vector3 position = rotation * invertedDistance + target.position;

                position.y += heightOffset; //HEIGHT OFFSET
                
                transform.rotation = rotation;
                transform.position = position;
                //transform.position = new Vector3(transform.position.x, transform.position.y + heightOffset, transform.position.z - distance);
                //transform.position = Vector3.Lerp(transform.position, position, followSpeed);
                //else
                //{
                //    transform.forward = target.forward;
                //    transform.position = Vector3.Lerp(transform.position, target.position + offset, followSpeed);
                //transform.rotation = target.rotation;
                //transform.LookAt(target.transform);
                //}
                //transform.RotateAround(target.transform.position, target.transform.up, turnSpeed * Time.deltaTime);
                //transform.rotation = target.transform.rotation;// Vector3.Lerp(transform.rotation.eulerAngles, target.rotation., turnSpeed);
            }
        }
    }
    
    //CLAMP ANGLE
    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F) angle += 360F;
        if (angle > 360F) angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
#endif
}