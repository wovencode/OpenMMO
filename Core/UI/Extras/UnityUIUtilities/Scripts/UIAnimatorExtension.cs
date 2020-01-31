using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class UIAnimatorExtension : BaseUIExtension
{
    public string animShowParam = "Show";
    private bool isShowing;
    private Animator cacheAnimator;
    public Animator CacheAnimator
    {
        get
        {
            if (cacheAnimator == null)
                cacheAnimator = GetComponent<Animator>();
            return cacheAnimator;
        }
    }

    private void Update()
    {
        CacheAnimator.SetBool(animShowParam, isShowing);
    }

    public override void Show()
    {
        isShowing = true;
    }

    public override void Hide()
    {
        isShowing = false;
    }

    public override bool IsVisible()
    {
        return isShowing;
    }
}
