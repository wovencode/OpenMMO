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
    [SerializeField] TextMeshPro nameField = null;
    [SerializeField] TextMeshPro guildField = null;
    [SerializeField] TextMeshPro zoneField = null;

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
                if (nameField != null && nameField.enabled) nameField.text = player.gameObject.name; //UPDATE PLAYER NAME
                if (guildField != null && guildField.enabled) guildField.text = "level " + player.level; //UPDATE PLAYER GUILD (TODO: temporarily just level for proof of concept)
                if (zoneField != null && zoneField.enabled) zoneField.text = player.currentZone.title; //UPDATE PLAYER ZONE
            }
        }
    }
#endif
}