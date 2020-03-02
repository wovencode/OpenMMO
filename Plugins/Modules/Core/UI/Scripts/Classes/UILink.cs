//by Fhiz
using UnityEngine;
using OpenMMO;
using OpenMMO.UI;

namespace OpenMMO.UI
{
	
	/// <summary>
    /// Attach to any game object that is part of a canvas. Allows to activate/deactivate any number of other UI elements when this one is activated/deactivated.
    /// </summary>
	public class UILink : MonoBehaviour
	{

        [Header("UI Links")]
        public GameObject[] uiLinks;
		
		/// <summary>
    	/// Shows all linked game objects when this one is activated.
    	/// </summary>
        private void OnEnable()
		{
			foreach (GameObject gameObject in uiLinks)
            {
            	if (gameObject != null)
                	gameObject.SetActive(true);
            }
        }
		
		/// <summary>
    	/// Hides all linked game objects when this one is deactivated.
    	/// </summary>
        private void OnDisable()
        {
            foreach (GameObject gameObject in uiLinks)
            {
            	if (gameObject != null)
                	gameObject.SetActive(false);
            }
        }
		
    }

}