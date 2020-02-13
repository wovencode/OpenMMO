// =======================================================================================
// UIPopupNotify
// by Weaver (Fhiz)
// MIT licensed
//
// This popup type offers no choices at all (not even Cancel/Close). Instead it simply
// displays a short message to the user and hides itself after a preset amount of seconds.
// This class is universal and can be used everywhere you want to display small pieces of
// information to your user, that require direct attention.
//
// =======================================================================================

using OpenMMO;
using OpenMMO.UI;
using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

namespace OpenMMO.UI
{

	// ===================================================================================
	// UIPopupNotify
	// ===================================================================================
	[DisallowMultipleComponent]
	public partial class UIPopupNotify: UIPopup
	{

		public static UIPopupNotify singleton;
		
		// -------------------------------------------------------------------------------
		// Awake
		// -------------------------------------------------------------------------------
		protected override void Awake()
		{
			singleton = this;
			base.Awake();
		}
		
		// -------------------------------------------------------------------------------
		// Init
		// -------------------------------------------------------------------------------
		public void Init(string _description, float _duration=2)
		{
			base.Init();
			Show(_description);
			Invoke(nameof(Close), _duration);
		}
		
		// -------------------------------------------------------------------------------
		// OnDestroy
		// -------------------------------------------------------------------------------
		void OnDestroy()
		{
			CancelInvoke(nameof(Close));
		}
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================