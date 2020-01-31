using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInputScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        InputManager.useMobileInputOnNonMobile = true;

    }
	
	// Update is called once per frame
	void Update ()
    {
        if (InputManager.GetButtonDown("Jump"))
            Debug.Log("Press Jump");
        if (InputManager.GetButtonUp("Jump"))
            Debug.Log("Release Jump");
        if (InputManager.GetButton("Jump"))
            Debug.Log("Hold Jump");

        float hAxis = InputManager.GetAxis("Horizontal", false);
        float vAxis = InputManager.GetAxis("Vertical", false);
        if (hAxis != 0)
            Debug.Log("hAxis " + hAxis);
        if (vAxis != 0)
            Debug.Log("vAxis " + vAxis);
    }
}
