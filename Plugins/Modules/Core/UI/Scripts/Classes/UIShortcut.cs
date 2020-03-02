//by Fhiz
using UnityEngine;
using UnityEngine.UI;
using OpenMMO;
using OpenMMO.UI;

namespace OpenMMO.UI
{
	
	/// <summary>
    /// Partial UIShortcut as base class for all short cut buttons. When pressed, the attached panel and/or game object is toggled active/inactive.
    /// </summary>
	[RequireComponent(typeof(Button))]
	public partial class UIShortcut : MonoBehaviour
	{

       	public Button button;
       	public UIRoot uiPanel;
       	public GameObject goPanel;
       	
        /// <summary>
    	/// When enabled, attaches the toggle method to the button of this shortcut.
    	/// </summary>
        void OnEnable()
		{
			button.onClick.SetListener(() => {
			
				if (uiPanel)
					uiPanel.Toggle();
				
				if (goPanel)
					goPanel.SetActive(!goPanel.activeSelf);

            });

        }
		
    }

}