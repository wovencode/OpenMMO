//BY DX4D
using UnityEngine;
using TMPro;
using OpenMMO;

/// <summary>Updates text fields to show this character's information.</summary>
public class NPCNameplateUpdater : MonoBehaviour
{
#pragma warning disable CS0414

	
	public CharacterProfile profile;

    [Header("UPDATE FREQUENCY")]
    [Tooltip("How many update frames must pass before this component updates again?")]
    [SerializeField] [Range(1, 60)] int tickFrequency = 30; //TICK RATE

    [Header("TEXT FIELDS")]
    [SerializeField] TextMeshPro nameField = null;
    [SerializeField] TextMeshPro levelField = null;
    
#pragma warning restore CS0414

#if _CLIENT
    int frameCount = 0; //FRAME COUNTER
    
    void FixedUpdate()
    {
        //if (!PlayerComponent.localPlayer) return; //NOT LOGGED IN

        frameCount++; //INCREMENT TICK

        if (frameCount >= tickFrequency) //TICK THIS FRAME?
        {
            frameCount = 0; //RESET THE COUNTER
			
            if (profile)
            {
                if (nameField != null && nameField.enabled)
                	nameField.text = profile.gameObject.name; //UPDATE PLAYER NAME
                	
                if (levelField != null && levelField.enabled)
                	levelField.text = "level " + profile.level; //UPDATE LEVEL
            }
        }
    }
#endif
}