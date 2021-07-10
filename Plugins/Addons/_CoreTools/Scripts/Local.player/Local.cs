//BY DX4D

using UnityEngine;
using Mirror;

public class Local : MonoBehaviour
{
    /// <summary>
    /// Get the local player object. Prevents frequent GetComponent calls and checks. Can be called from anywhere.
    /// </summary>
    //public static GameObject player => ClientScene.localPlayer ? ClientScene.localPlayer.gameObject : null; //REMOVED - DX4D
    public static GameObject player => NetworkClient.localPlayer ? NetworkClient.localPlayer.gameObject : null; //ADDED - DX4D
}
