//BY DX4D
using UnityEngine;
using OpenMMO;

/// <summary>Carries a set of camera prefabs that are loaded from the Resources/Cameras folder.</summary>
public class CameraDolly : MonoBehaviour
{
#pragma warning disable CS0414
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
    [SerializeField] bool cameraSpawned = false;
    [SerializeField] GameObject spawnedCamera;
#pragma warning restore CS0414

#if _CLIENT
#if UNITY_EDITOR
    private void OnValidate()
    {
        UpdateAttachedCamera();
    }
#endif

    //ON DESTROYED
    private void OnDestroy()
    {
        DestroySpawnedCamera();
    }

    //UPDATE
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

    //UPDATE ATTACHED CAMERA
    bool UpdateAttachedCamera()
    {
        if (!activeCamera || !activeCamera.name.Contains(cameraType.ToString() + "Camera")) //NOTE: We could use .ToUpper() so this is not case sensitive, but for performance reasons we do not...keep this in mind if you add new cameras.
        {
            OnCameraChanged();
            return true;
        }

        return false;
    }

    //DESTROY SPAWNED CAMERA
    void DestroySpawnedCamera()
    {
        if (Application.isPlaying)
        {
            if (spawnedCamera) GameObject.Destroy(spawnedCamera); //DESTROY ACTIVE CAMERA
            //if (activeCamera) GameObject.DestroyImmediate(activeCamera, true); //DESTROY ACTIVE CAMERA
            //if (activeCamera) activeCamera.SetActive(false); //DISABLE ACTIVE CAMERA
        }
    }

    //ON CAMERA CHANGED
    void OnCameraChanged()
    {
        DestroySpawnedCamera();

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