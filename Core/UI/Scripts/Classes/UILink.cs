
using UnityEngine;
using OpenMMO;
using OpenMMO.UI;

namespace OpenMMO.UI
{
	
	// ===================================================================================
	// UILink
	// ===================================================================================
	public class UILink : MonoBehaviour
	{

        [Header("UI Links")]
        public GameObject[] uiLinks;
		
		// -------------------------------------------------------------------------------
        private void OnEnable()
		{
			foreach (GameObject gameObject in uiLinks)
            {
            	if (gameObject != null)
                	gameObject.SetActive(true);
            }
        }
		
		// -------------------------------------------------------------------------------
        private void OnDisable()
        {
            foreach (GameObject gameObject in uiLinks)
            {
            	if (gameObject != null)
                	gameObject.SetActive(false);
            }
        }
		
		// -------------------------------------------------------------------------------
		
    }

}

// =======================================================================================