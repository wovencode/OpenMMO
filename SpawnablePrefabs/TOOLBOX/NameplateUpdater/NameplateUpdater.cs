//BY: Davil [DX4D]
using UnityEngine;
using TMPro;
using OpenMMO;

/// <summary>Updates text fields to show this player's information.</summary>
public class NameplateUpdater : MonoBehaviour
{
    [Header("UPDATE FREQUENCY")]
    [Tooltip("How many FixedUpdate frames must pass before this component updates again?")]
    [SerializeField] [Range(1, 60)] int tickFrequency = 30; //TICK RATE

    [Header("TEXT FIELDS")]
    [SerializeField] TextMeshPro nameField;
    [SerializeField] TextMeshPro guildField;
    //[SerializeField] TextMeshPro titleField;

#if _CLIENT
    int frameCount = 0; //FRAME COUNTER
    void FixedUpdate()
    {
        //if (!PlayerComponent.localPlayer) return; //NOT LOGGED IN

        frameCount++; //INCREMENT TICK

        if (frameCount >= tickFrequency) //TICK THIS FRAME?
        {
            frameCount = 0; //RESET THE COUNTER

            PlayerComponent player = transform.parent.gameObject.GetComponent<PlayerComponent>();

            if (player)
            {
                if (nameField != null) nameField.text = player.gameObject.name; //UPDATE PLAYER NAME
                if (guildField != null) guildField.text = "level " + player.level; //UPDATE PLAYER GUILD (TODO: temporarily just level for proof of concept)
            }
        }
    }
#endif
}