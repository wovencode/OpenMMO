//BY DX4D
using UnityEngine;

[System.Serializable]
public struct DialogueScript
{
    [SerializeField, TextArea(1, 5)] string[] _lines;
    public string[] lines { get { return _lines; } }


}
