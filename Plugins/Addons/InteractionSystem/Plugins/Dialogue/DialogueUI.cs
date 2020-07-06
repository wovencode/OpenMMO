//BY DX4D
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

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
            if (_dialogue != null) dialogueChanged = true;
        }
    }
    [SerializeField] bool dialogueChanged = true;

    [SerializeField] GameObject dialoguePanel;
    [SerializeField] internal Image speakerImage;
    [SerializeField] internal Text speakerTitle;
    [SerializeField] internal Text dialogueLine;
    [SerializeField] internal Button nextButton;

    private void OnValidate()
    {
        //if (dialogue == null) { dialogue = gameObject.GetComponent<Dialogue>(); }
    }

    private void UpdateFields()
    {
        if (dialogue != null)
        {
            speakerImage.sprite = dialogue.icon;
            speakerTitle.text = dialogue.title;
            dialogueLine.text = string.Empty; //clear lines

            System.Text.StringBuilder dialogueBlock = new System.Text.StringBuilder();

            for (int i = 0; i < dialogue.dialogue.lines.Length; i++)
            {
                dialogueBlock.AppendLine(dialogue.dialogue.lines[i]);
                //dialogueLine.text += ((i > 0) ? "\n" : "") + dialogue.dialogue.lines[i];
            }

            dialogueLine.text = dialogueBlock.ToString();

            //dialogueChanged = true;
        }
        //foreach (string line in dialogue.dialogue.lines)
        //{
        //    dialogueLine.text += "\n" + line;
        //}

    }

    private void ShowDialogue() { dialoguePanel.SetActive(true); }
    private void HideDialogue() { dialoguePanel.SetActive(false); }
    [Header("DISABLE IN:")]
    [Tooltip("The amount of time (in seconds) before the attached gameobject is deactivated.")]
    [SerializeField] double duration = 4.0f;
    double endTime = -1;

    void Awake()
    {
        endTime = NetworkTime.time + duration;//Time.fixedTime
    }

    void Update()
    {
        if (endTime < 0 && dialoguePanel.activeSelf) { endTime = NetworkTime.time + duration; }

        if (NetworkTime.time > endTime) { endTime = -1; HideDialogue(); }
        
        //if (!dialogueChanged) return; //NOTHING CHANGED - No update needed

        //if (dialogueChanged) UpdateFields();

        if (dialogue == null) //NO DIALOGUE - No update needed
        {
            //if (dialogueChanged) UpdateFields();
            if (dialoguePanel.activeSelf) HideDialogue();
            return;
        }
        else
        {
            //if (dialogueChanged)
            //{
            if (dialogueChanged)
            {
                UpdateFields();
                if (!dialoguePanel.activeSelf) ShowDialogue();
                dialogueChanged = false;
            }
            //}

        }
    }
}
