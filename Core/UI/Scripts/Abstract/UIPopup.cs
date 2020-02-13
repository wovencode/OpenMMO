// =======================================================================================
// UIPopup
// by Weaver (Fhiz)
// MIT licensed
//
// The base class all popup windows are derived from. It features a UIWindowBackground
// that hides all other UI elements underneath it (so the user can only interact with
// the popup while it is shown).
//
// Popup classes are partial but feature no DevExtension hooks as this impacts performance
// Instead, developers are encouraged to add their own components to the same UI element.
//
// =======================================================================================

using OpenMMO;
using OpenMMO.UI;
using OpenMMO.Network;
using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

namespace OpenMMO.UI
{
	
	// ===================================================================================
	// UIPopup
	// ===================================================================================
	public abstract partial class UIPopup : UIBase
	{
		
		[Header("UI Popup")]
		[SerializeField] protected Animator animator;
		[SerializeField] protected string showTriggerName = "show";
		[SerializeField] protected string closeTriggerName = "close";
		[SerializeField] protected Text description;
		
		// -------------------------------------------------------------------------------
		protected override void Awake()
		{
			base.Awake();
		}
		
		// -------------------------------------------------------------------------------
		public virtual void Show(string _text)
		{	
			description.text = _text;
			base.Show();
			Init();
		}
		
		// -------------------------------------------------------------------------------
		protected void Init()
		{
			animator.SetTrigger(showTriggerName);
			
			if (UIBackgroundLayer.singleton != null)
				UIBackgroundLayer.singleton.FadeIn();
		}
		
		// -------------------------------------------------------------------------------
		public override void Hide()
		{
			Close();
			base.Hide();
		}
		
		// -------------------------------------------------------------------------------
		public virtual void onClickConfirm()
		{
			Close();
			base.Hide();
		}
		
		// -------------------------------------------------------------------------------
		public virtual void onClickCancel()
		{
			Close();
			base.Hide();
		}
		
		// -------------------------------------------------------------------------------
		public void Close()
		{
			
			if (UIBackgroundLayer.singleton != null)
				UIBackgroundLayer.singleton.FadeOut();
			
			animator.SetTrigger(closeTriggerName);
			
		}

		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================