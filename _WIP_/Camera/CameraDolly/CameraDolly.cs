//BY: Davil [DX4D]
using UnityEngine;
using OpenMMO;

/// <summary>Carries a set of camera prefabs that are loaded from the Resources/Cameras folder.</summary>
public class CameraDolly : MonoBehaviour
{
    [Header("UPDATE FREQUENCY")]
    [Tooltip("How many update frames must pass before this component updates again?")]
    [SerializeField] [Range(1, 60)] int tickFrequency = 60; //TICK RATE

    [Header("CAMERA SELECT")]
    [Tooltip("Select a camera to load.")]
    [SerializeField] CameraType cameraType = CameraType.Malice;
    [Tooltip("Parent the camera to this object.")]
    public bool attachCameraToMe = false;

    [Header("-read only-")]
    [Tooltip("The currently active camera. (Set by the camera type)")]
    [SerializeField] GameObject activeCamera;
    bool cameraSpawned = false;
    GameObject spawnedCamera;


#if _CLIENT
#if UNITY_EDITOR
    private void OnValidate()
    {
        UpdateAttachedCamera();
    }
#endif

    private void Awake()
    {
    }

    int frameCount = 0; //FRAME COUNTER
    void FixedUpdate()
    {
        frameCount++; //INCREMENT TICK

        if (frameCount >= tickFrequency) //TICK THIS FRAME?
        {
            frameCount = 0; //RESET THE COUNTER

            if (UpdateAttachedCamera()) //UPDATE CAMERA
            {
            }

            if (!cameraSpawned && activeCamera) //SPAWN CAMERA
            {
                SpawnCamera();
            }
        }
    }

    bool UpdateAttachedCamera()
    {
        if (!activeCamera || !activeCamera.name.Contains(cameraType.ToString() + "Camera")) //NOTE: We could use .ToUpper() so this is not case sensitive, but for performance reasons we do not...keep this in mind if you add new cameras.
        {
            OnCameraChanged();
            return true;
        }

        return false;
    }

    //ON CAMERA CHANGED
    void OnCameraChanged()
    {
        if (Application.isPlaying)
        {
            //TODO: We really want to destroy this, but the logic needs to be different
            //if (activeCamera) GameObject.DestroyImmediate(activeCamera, true); //DESTROY ACTIVE CAMERA
            if (spawnedCamera) GameObject.Destroy(spawnedCamera); //DESTROY ACTIVE CAMERA
            //if (activeCamera) activeCamera.SetActive(false); //DISABLE ACTIVE CAMERA
        }

        activeCamera = Resources.Load<GameObject>("Cameras/" + cameraType.ToString() + "Camera"); //LOAD NEW ACTIVE CAMERA
        cameraSpawned = false;
    }

    //SPAWN CAMERA
    void SpawnCamera()
    {
        if (!Application.isPlaying) return; //DON'T SPAWN IF NOT PLAYING

        if (attachCameraToMe) { spawnedCamera = Instantiate<GameObject>(activeCamera, transform); } //INSTANTIATE (PARENTED)
        else { spawnedCamera = Instantiate<GameObject>(activeCamera); } //INSTANTIATE (UNPARENTED)

        cameraSpawned = true;

        if (Camera.main && Camera.main.name.Contains("Main")) { Camera.main.gameObject.SetActive(false); }
    }
#endif
}