//by Fhiz
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OpenMMO;
using OpenMMO.UI;

namespace OpenMMO.UI
{
	
	/// <summary>
    /// Attach this MonoBehaviour to any game object that holds one or more UIButtons to add group functions to them. This allows to disable all buttons permanently or temporarily when one of them is pressed.
    /// </summary>
	public partial class UIButtonGroup : MonoBehaviour
	{
	
		[HideInInspector] public List<UIButton> buttons = new List<UIButton>();
		
		/// <summary>
    	/// When called, clears all buttons in the list.
    	/// </summary>
		public void Clear()
		{
			buttons.Clear();
		}
		
		/// <summary>
    	/// Adds a UIButton object to the list.
    	/// </summary>
		public void Add(UIButton button)
		{
			buttons.Add(button);
		}
		
		/// <summary>
    	/// Called when just one of the buttons is pressed to call OnPressed(true) on all others.
    	/// </summary>
		public void OnPressed(UIButton _button)
		{
            foreach (UIButton button in buttons)
            {
                button.OnPressed(button != _button);
            }
		}
		
	}

}