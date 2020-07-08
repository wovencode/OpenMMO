//BY DX4D
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    //ICON
    [SerializeField] Sprite _icon = null;
    public Sprite icon { get { return _icon; } }
    //TITLE
    [SerializeField] string _title = string.Empty;
    public string title { get { return _title; } }
    //DIALOGUE
    [SerializeField] DialogueScript _dialogue = new DialogueScript();
    public DialogueScript dialogue { get { return _dialogue; } }

    //[SerializeField, TextArea(1, 5)] string[] _lines;
    //public string[] lines { get { return _lines; } }
}
