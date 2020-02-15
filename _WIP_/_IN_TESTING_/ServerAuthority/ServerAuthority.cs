//BY DX4D
using UnityEngine;

[CreateAssetMenu(menuName = "OpenMMO/Config/Server Authority")]
public class ServerAuthority : ScriptableObject
{
    [Header("SERVER SIDE AUTHORITY - movement")]
    [Tooltip("The level of validation to player movement desired." +
        "\nComplete: Validates the entire transform - Warps the player instantly back in position if they stray."
        + "Tolerant: Validates (only) positon with tolerance factored in - can return the player to the destired postion smoothly. - preferred"
        + "Low: Just validates position and nothing else.")]
    [SerializeField] public ValidationLevel validation = ValidationLevel.Complete;
    [Tooltip("When using Tolerant validation, a value will be tolerated if it is out of range by up to this amount in each direction.")]
    [SerializeField] public float tolerence = 7f;
    [Tooltip("Smoothly Move back to the server dictated position.")]
    [SerializeField] public bool smooth = true;
    [Tooltip("How quickly the server will smooth player movement.")]
    [SerializeField] public float smoothing = 1f;
}