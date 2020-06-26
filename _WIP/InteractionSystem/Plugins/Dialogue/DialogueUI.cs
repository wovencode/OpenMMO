//BY DX4D
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] internal Dialogue _dialogue;
    public Dialogue dialogue
    {
        get { return _dialogue; }
        set
        {
            //if (_dialogue == value) return;
            _dialogue = value;
            dialogueChanged = true;
        }
    }
    bool dialogueChanged = true;

    [SerializeField] GameObject dialoguePanel;
    [SerializeField] internal Image speakerImage;
    [SerializeField] internal Text speakerTitle;
    [SerializeField] internal Text dialogueLine;
    [SerializeField] internal Button nextButton;

    private void OnValidate()
    {
        if (dialogue == null) { dialogue = gameObject.GetComponent<Dialogue>(); }
    }

    private void UpdateFields()
    {
        speakerImage.sprite = dialogue.icon;
        speakerTitle.text = dialogue.title;
        dialogueLine.text = string.Empty; //clear lines

        for (int i = 0; i < dialogue.dialogue.lines.Length; i++)
        {
            dialogueLine.text += ((i > 0) ? "\n" : "") + dialogue.dialogue.lines[i];
        }
        //foreach (string line in dialogue.dialogue.lines)
        //{
        //    dialogueLine.text += "\n" + line;
        //}

        dialogueChanged = false;
    }

    private void Awake()
    {
        dialogueChanged = true;
    }
    private void OnDisable()
    {
        dialogueChanged = true;
    }
    private void FixedUpdate()
    {
        if (!dialogueChanged) return; //NOTHING CHANGED - No update needed
        if (dialogue == null) //NO DIALOGUE - No update needed
        {
            if (dialoguePanel.activeSelf) { dialoguePanel.SetActive(false); }
            return;
        }
        else if (!dialoguePanel.activeSelf) { dialoguePanel.SetActive(true); }

        UpdateFields();
    }
}
