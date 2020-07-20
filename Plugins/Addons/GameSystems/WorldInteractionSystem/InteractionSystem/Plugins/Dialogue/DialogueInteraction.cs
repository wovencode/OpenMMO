//BY DX4D
using System.Collections.Generic;
using UnityEngine;

namespace OpenMMO
{
    public class DialogueInteraction : Interaction
    {
        public DialogueUI ui;
        
        //DIALOGUE
        [SerializeField] Queue<string> _dialogue = new Queue<string>();
        public Queue<string> dialogue { get { return _dialogue; } }

        public override void ClientAction(GameObject user) { ClientResponse(); }
        public override void ServerAction(GameObject user) { }

        public void ClientResponse()
        {
            //GET DIALOGUE COMPONENT
            Dialogue[] dialogues = gameObject.GetComponents<Dialogue>();
            if (dialogues.Length == 0) return; //NO DIALOGUES

            if (!ui)
            {
                ui = FindObjectOfType<DialogueUI>();
            }
            //ASSIGN UI HEADER VARIABLES
            if (ui != null)
            {
                //ITERATE THROUGH SPEECHES
                foreach (Dialogue speech in dialogues)
                {
                    if (speech.isActiveAndEnabled) ui.dialogue = speech;
                    //ui.speakerImage.sprite = speech.icon; //SET UI IMAGE
                    //ui.speakerTitle.text = speech.title; //SET UI TITLE
                }

                //QUEUE SPEECH LINES
                //foreach (string line in speech.lines)
                //{
                //if (line.StartsWith("?")) { } //Use later for Dialogue Options
                //    _dialogue.Enqueue(line);
                //}
            }

            if (_dialogue.Count < 1) return; //NO DIALOGUE

            //if (ui != null)
            //{
            //    foreach (string dialogueLine in dialogue)
            //    {
                    //TODO: This does not seem to delete
                    //ui.dialogueLine.text += _dialogue.Dequeue(); //SET UI DIALOGUE LINE
            //    }
            //}

            #region DEBUG
#if UNITY_EDITOR && DEBUG
            System.Text.StringBuilder log = new System.Text.StringBuilder((ui != null) ? ui.speakerTitle.text.ToString() : "");
            foreach (string line in dialogue)
            {
                log.AppendLine(line);
            }
            Debug.Log(log.ToString());
#endif
            #endregion //DEBUG
        }
    }
}