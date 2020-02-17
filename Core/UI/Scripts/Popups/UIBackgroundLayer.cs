// =======================================================================================
// Attached to a UI element that stretches across the entire screen in order to hide
// all other UI elements underneath it. This is used as a background for all kinds of
// popup windows. It blocks ray-casting, so the user may only interact with the popup
// instead of clicking on other UI elements.
//
// =======================================================================================

using UnityEngine;
using System.Collections;
using OpenMMO;
using OpenMMO.UI;

namespace OpenMMO.UI
{
	
	// ===================================================================================
	// UIBackgroundLayer
	// ===================================================================================
	public partial class UIBackgroundLayer : UIRoot 
	{
		
		[SerializeField] protected GameObject blackBackground;
		[SerializeField] protected Animator animator;
		[SerializeField] protected string fadeInTriggerName = "fadeIn";
		[SerializeField] protected string fadeOutTriggerName = "fadeOut";
		
		public static UIBackgroundLayer singleton;
		
		// -------------------------------------------------------------------------------
		// Awake
		// -------------------------------------------------------------------------------
		protected override void Awake()
		{
			singleton = this;
			base.Awake();
		}
		
		// -------------------------------------------------------------------------------
		// BlackIn
		// Immediately shows the full black background (no fading animation)
		// -------------------------------------------------------------------------------
		public void BlackIn(float duration=0f)
		{
			
			blackBackground.SetActive(true);
			
			if (duration > 0)
				Invoke(nameof(BlackOutDelayed), duration);
		}
		
		// -------------------------------------------------------------------------------
		// FadeIn
		// -------------------------------------------------------------------------------
		public void FadeIn(float delay=0f)
		{
			if (delay > 0)
				Invoke(nameof(FadeInDelayed), delay);
			else
				FadeInDelayed();
		}
		
		// -------------------------------------------------------------------------------
		// FadeInDelayed
		// -------------------------------------------------------------------------------
		protected void FadeInDelayed()
		{
			if (root.activeSelf)
				animator.SetTrigger(fadeInTriggerName);
			Show();
		}
		
		// -------------------------------------------------------------------------------
		// BlackOut
		// Immediately hides the full black background (no fading animation)
		// -------------------------------------------------------------------------------
		public void BlackOut(float delay=0f)
		{
			if (delay > 0)
				Invoke(nameof(BlackOutDelayed), delay);
			else
				blackBackground.SetActive(false);
		}
		
		// -------------------------------------------------------------------------------
		// BlackOutDelayed
		// -------------------------------------------------------------------------------
		protected void BlackOutDelayed()
		{
			blackBackground.SetActive(false);
		}
		
		// -------------------------------------------------------------------------------
		// FadeOut
		// -------------------------------------------------------------------------------
		public void FadeOut(float delay=0f)
		{
			if (delay > 0)
				Invoke(nameof(FadeOutDelayed), delay);
			else
				FadeOutDelayed();
		}
		
		// -------------------------------------------------------------------------------
		// FadeOutDelayed
		// -------------------------------------------------------------------------------
		protected void FadeOutDelayed()
		{
		
			if (root.activeSelf)
				animator.SetTrigger(fadeOutTriggerName);
			
			StartCoroutine(nameof(DeactivateWindow));
		}
		
		// -------------------------------------------------------------------------------
		protected IEnumerator DeactivateWindow()
		{
			
			float delay = 0;
			
			if (root.activeSelf)
				delay = animator.GetCurrentAnimatorStateInfo(0).length;
				
			yield return new WaitForSeconds(delay);
			Hide();
		}
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================