//by Fhiz
using OpenMMO;
using OpenMMO.UI;
using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

namespace OpenMMO.UI
{

	/// <summary>
    /// This popup type offers no choices at all (not even Cancel/Close). Instead it simply
	/// displays a short message to the user and hides itself after a preset amount of seconds.
	/// This class is universal and can be used everywhere you want to display small pieces of
	/// information to your user, that require direct attention.
    /// </summary>
	[DisallowMultipleComponent]
	public partial class UIPopupNotify: UIPopup
	{

		public static UIPopupNotify singleton;
		
		/// <summary>
    	/// Awake sets the singleton (as this popup is unique) and calls base.Awake
    	/// </summary>
		protected override void Awake()
		{
			singleton = this;
			base.Awake();
		}
		
		/// <summary>
    	/// Initializes the popup by setting its text and close duration.
    	/// </summary>
		public void Init(string _description, float _duration=2, bool fade=true)
		{
			Show(_description, fade);
			Invoke(nameof(Close), _duration);
		}
		
		/// <summary>
    	/// Cancel the invoke when this object is disabled.
    	/// </summary>
		void OnDisable()
		{
			CancelInvoke(nameof(Close));
		}
		
		/// <summary>
    	/// Cancel the invoke when this object is destroyed.
    	/// </summary>
		void OnDestroy()
		{
			CancelInvoke(nameof(Close));
		}
		
	}

}