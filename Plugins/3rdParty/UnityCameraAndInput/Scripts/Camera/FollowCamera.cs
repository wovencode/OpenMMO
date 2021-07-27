using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
public class FollowCamera : MonoBehaviour
{
    public Camera targetCamera;
    public Transform target;
    public Vector3 targetOffset;
    [Header("Rotation")]
    public float xRotation;
    public float yRotation;
    [Tooltip("If this is TRUE, will update Y-rotation follow target")]
    public bool useTargetYRotation;
    [Header("Zoom")]
    public float zoomDistance = 10.0f;
    [Header("Zoom by ratio")]
    public bool zoomByAspectRatio;
    public List<ZoomByAspectRatioSetting> zoomByAspectRatioSettings = new List<ZoomByAspectRatioSetting>()
    {
        new ZoomByAspectRatioSetting() { width = 16, height = 9, zoomDistance = 0.0001f },
        new ZoomByAspectRatioSetting() { width = 16, height = 10, zoomDistance = 1.75f },
        new ZoomByAspectRatioSetting() { width = 3, height = 2, zoomDistance = 3f },
        new ZoomByAspectRatioSetting() { width = 4, height = 3, zoomDistance = 5.5f },
        new ZoomByAspectRatioSetting() { width = 5, height = 4, zoomDistance = 7 },
    };
    [Header("Wall hit spring")]
    public bool enableWallHitSpring;
    public LayerMask wallHitLayerMask = -1;
    public QueryTriggerInteraction wallHitQueryTriggerInteraction = QueryTriggerInteraction.Ignore;

    // Properties
    public Transform CacheTransform { get; private set; }
    public Transform CacheCameraTransform { get; private set; }

    public Camera CacheCamera
    {
        get { return targetCamera; }
    }
    
    // Improve Garbage collector
    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private float targetYRotation;
    private Vector3 wantedPosition;
    private float wantedYRotation;
    private float windowaspect;
    private Quaternion wantedRotation;
    private Ray tempRay;
    private RaycastHit[] tempHits;
    private float tempDistance;

    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        Gizmos.color = Color.red;
        Gizmos.DrawLine(tempRay.origin, tempRay.origin + tempRay.direction * tempDistance);
#endif
    }

    protected virtual void Awake()
    {
        PrepareComponents();
    }

    private void PrepareComponents()
    {
        CacheTransform = transform;
        if (targetCamera == null)
            targetCamera = GetComponent<Camera>();
        CacheCameraTransform = CacheCamera.transform;
    }

    protected virtual void LateUpdate()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
            PrepareComponents();
#endif
        targetPosition = target == null ? Vector3.zero : target.position;
        Vector3 upVector = target == null ? Vector3.up : target.up;
        targetPosition += (targetOffset.x * CacheTransform.right) + (targetOffset.y * upVector) + (targetOffset.z * CacheTransform.forward);
        targetYRotation = target == null ? 0 : target.eulerAngles.y;

        if (zoomByAspectRatio)
        {
            windowaspect = CacheCamera.aspect;
            zoomByAspectRatioSettings.Sort();
            foreach (ZoomByAspectRatioSetting data in zoomByAspectRatioSettings)
            {
                if (windowaspect + windowaspect * 0.025f > data.Aspect() &&
                    windowaspect - windowaspect * 0.025f < data.Aspect())
                {
                    zoomDistance = data.zoomDistance;
                    break;
                }
            }
        }

        if (zoomDistance == 0f)
            zoomDistance = 0.0001f;

        if (CacheCamera.orthographic)
            CacheCamera.orthographicSize = zoomDistance;

        wantedYRotation = useTargetYRotation ? targetYRotation : yRotation;

        // Convert the angle into a rotation
        targetRotation = Quaternion.Euler(xRotation, wantedYRotation, 0);
        wantedRotation = targetRotation;

        // Set the position of the camera on the x-z plane to:
        // distance meters behind the target
        wantedPosition = targetPosition - (wantedRotation * Vector3.forward * -zoomDistance);

        if (enableWallHitSpring)
        {
            float nearest = float.MaxValue;
            tempRay = new Ray(targetPosition, wantedRotation * -Vector3.forward);
            tempDistance = Vector3.Distance(targetPosition, wantedPosition);
            tempHits = Physics.RaycastAll(tempRay, tempDistance, wallHitLayerMask, wallHitQueryTriggerInteraction);
            for (int i = 0; i < tempHits.Length; i++)
            {
                if (tempHits[i].distance < nearest)
                {
                    nearest = tempHits[i].distance;
                    wantedPosition = tempHits[i].point;
                }
            }
        }

        // Update position & rotation
        CacheTransform.position = wantedPosition;
        CacheTransform.rotation = wantedRotation;
    }

    [System.Serializable]
    public struct ZoomByAspectRatioSetting : System.IComparable
    {
        public float width;
        public float height;
        public float zoomDistance;

        public float Aspect()
        {
            return (width / height);
        }

        public int CompareTo(object obj)
        {
            if (!(obj is ZoomByAspectRatioSetting))
                return 0;
            return Aspect().CompareTo(((ZoomByAspectRatioSetting)obj).Aspect());
        }
    }
}
