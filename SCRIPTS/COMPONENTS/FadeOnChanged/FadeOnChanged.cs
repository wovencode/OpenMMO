//BY DX4D
using System.Collections;
using UnityEngine;
using TMPro;
using OpenMMO;

/// <summary>Watches a text field and display it for a period of time whenever it changes.</summary>
public class FadeOnChanged : MonoBehaviour
{
#pragma warning disable CS0414
    [Header("UPDATE FREQUENCY")]
    [Tooltip("How many update frames must pass before this component updates again?")]
    [SerializeField] [Range(1, 60)] int tickFrequency = 15; //TICK RATE


    [Header("TEXT FIELDS")]
    [SerializeField] TextMeshPro textField = null;

    //[Tooltip("If this is set, only the local player will see the popup.")]
    //[SerializeField] bool showLocalOnly = true;
    [Header("FADE IN/OUT DURATION")]
    [Tooltip("How fast this the text will fade in and out.")]
    [SerializeField] [Range(0.01f, 1.00f)] float fadeSpeed = 0.05f;
    [Tooltip("Once fully visible, display for this amount of time (in seconds) before fading.")]
    [SerializeField] float visibleDuration = 3f;
#pragma warning restore CS0414

#if _CLIENT
    string formerValue = string.Empty;

    int frameCount = 0; //FRAME COUNTER
    void FixedUpdate()
    {
        //if (!PlayerComponent.localPlayer) return; //NOT LOGGED IN

        frameCount++; //INCREMENT TICK

        if (frameCount >= tickFrequency) //TICK THIS FRAME?
        {
            frameCount = 0; //RESET THE COUNTER
            
            if (textField != null)
            {
                if (textField.text != formerValue)
                {
                    formerValue = textField.text;
                    Show();
                }
            }
                // gameObject.transform.forward = Camera.main.transform.forward; //FACE THE CAMERA
        }
    }

    //FADE
    void Show()
    {
        //if (showLocalOnly && !transform.parent.gameObject.GetComponent<PlayerComponent>().isLocalPlayer) return;
        //if (showLocalOnly && PlayerComponent.localPlayer && !transform.parent.name.Contains(PlayerComponent.localPlayer.name)) return;
        StartCoroutine("FadeIn");
    }

    //FADE IN
    IEnumerator FadeIn()
    {
        textField.enabled = true;
        for (float fadetime = 0f; fadetime <= 1f; fadetime += fadeSpeed)
        {
            Color color = textField.color;
            color.a = fadetime;
            textField.color = color;
            yield return new WaitForSeconds(fadeSpeed);
        }
        
        yield return new WaitForSeconds(visibleDuration); //WAIT TO FADE AWAY
        StartCoroutine("FadeOut");
    }

    //FADE OUT
    IEnumerator FadeOut()
    {
        for (float fadetime = 1f; fadetime >= 0f; fadetime -= fadeSpeed)
        {
            Color color = textField.color;
            color.a = fadetime;
            textField.color = color;
            yield return new WaitForSeconds(fadeSpeed);
        }
        textField.enabled = false;
    }

#endif
}