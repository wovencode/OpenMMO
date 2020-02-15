//BY DX4D
using UnityEngine;

public enum ValidationLevel
{
    [Tooltip("Validates the entire transform.\nWarps the player instantly back in position if they stray.")]
    Complete,

    [Tooltip("Validates positon with tolerance factored in.\nReturns the player to the destired postion smoothly.\n[performance mode]")]
    Tolerant,

    [Tooltip("Just validates position and nothing else.")]
    Low,

    [Tooltip("Does not validate movement on the server.")]
    None
}