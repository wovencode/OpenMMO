
using UnityEngine;
using UnityEngine.UI;
using OpenMMO;
using OpenMMO.UI;

namespace OpenMMO.UI
{
	
	// ===================================================================================
	// UIShortcut
	// ===================================================================================
	[RequireComponent(typeof(Button))]
	public partial class UIShortcut : MonoBehaviour
	{

       	public Button button;
       	public UIRoot uiPanel;
       	public GameObject goPanel;
       	
       	// -------------------------------------------------------------------------------
       	// OnEnable
       	// -------------------------------------------------------------------------------
        void OnEnable()
		{
			button.onClick.SetListener(() => {
			
				if (uiPanel)
					uiPanel.Toggle();
				
				if (goPanel)
					goPanel.SetActive(!goPanel.activeSelf);

            });

        }
		
		// -------------------------------------------------------------------------------
		
    }

}

// =======================================================================================