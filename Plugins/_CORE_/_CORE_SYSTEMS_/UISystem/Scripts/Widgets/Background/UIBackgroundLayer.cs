//by Fhiz
using UnityEngine;
using System.Collections;
using OpenMMO;
using OpenMMO.UI;

namespace OpenMMO.UI
{
	
	/// <summary>
    /// Attached to a UI element that stretches across the entire screen in order to hide
	/// all other UI elements underneath it. This is used as a background for all kinds of
	/// popup windows. It blocks ray-casting, so the user may only interact with the popup
	/// instead of clicking on other UI elements.
    /// </summary>
	public partial class UIBackgroundLayer : UIRoot 
	{
		
		[SerializeField] protected GameObject blackBackground;
		[SerializeField] protected Animator animator;
		[SerializeField] protected string fadeInTriggerName = "fadeIn";
		[SerializeField] protected string fadeOutTriggerName = "fadeOut";
		
		public static UIBackgroundLayer singleton;
		
		/// <summary>
    	/// Awake sets the singleton (as this popup is unique) and calls base.Awake
    	/// </summary>
		protected override void Awake()
		{
			singleton = this;
			base.Awake();
		}
		
		/// <summary>
    	/// Immediately shows the full black background (no fading animation)
    	/// </summary>
		public void BlackIn(float duration=0f)
		{
			
			blackBackground.SetActive(true);
			
			if (duration > 0)
				Invoke(nameof(BlackOutDelayed), duration);
		}
		
		/// <summary>
    	/// Shows the background with a smooth fade-in effect.
    	/// </summary>
		public void FadeIn(float delay=0f)
		{
			if (delay > 0)
				Invoke(nameof(FadeInDelayed), delay);
			else
				FadeInDelayed();
		}
		
		/// <summary>
    	/// Fades the background in after a certain delay.
    	/// </summary>
		protected void FadeInDelayed()
		{
			if (root.activeSelf)
				animator.SetTrigger(fadeInTriggerName);
			Show();
		}
		
		/// <summary>
    	/// Immediately hides the full black background (no fading animation)
    	/// </summary>
		public void BlackOut(float delay=0f)
		{
			if (delay > 0)
				Invoke(nameof(BlackOutDelayed), delay);
			else
				blackBackground.SetActive(false);
		}
		
		/// <summary>
    	/// Hides the black background immediately again after a delay.
    	/// </summary>
		protected void BlackOutDelayed()
		{
			blackBackground.SetActive(false);
		}
		
		/// <summary>
    	/// Smoothly fades out the black background.
    	/// </summary>
		public void FadeOut(float delay=0f)
		{
			if (delay > 0)
				Invoke(nameof(FadeOutDelayed), delay);
			else
				FadeOutDelayed();
		}
		
		/// <summary>
    	/// Smoothly fades out the black background after a delay.
    	/// </summary>
		protected void FadeOutDelayed()
		{
		
			if (root.activeSelf)
				animator.SetTrigger(fadeOutTriggerName);
			
			StartCoroutine(nameof(DeactivateWindow));
		}
		
		/// <summary>
    	/// Used to deactivate the black background window that blocks raycast after a certain delay.
    	/// </summary>
		protected IEnumerator DeactivateWindow()
		{
			
			float delay = 0;
			
			if (root.activeSelf)
				delay = animator.GetCurrentAnimatorStateInfo(0).length;
				
			yield return new WaitForSeconds(delay);
			Hide();
		}
		
	}

}