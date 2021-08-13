using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(RectTransform))]
public class UIFollowWorldPosition : MonoBehaviour
{
    public Vector3 targetPosition;
    private bool alreadyShown;
    private CanvasGroup tempCanvasGroup;
    public CanvasGroup TempCanvasGroup
    {
        get
        {
            if (tempCanvasGroup == null)
                tempCanvasGroup = GetComponent<CanvasGroup>();
            return tempCanvasGroup;
        }
    }

    private RectTransform tempTransform;
    public RectTransform TempTransform
    {
        get
        {
            if (tempTransform == null)
                tempTransform = GetComponent<RectTransform>();
            return tempTransform;
        }
    }

    private void Awake()
    {
        TempCanvasGroup.alpha = 0;
    }

    private void LateUpdate()
    {
        UpdatePosition();
        if (!alreadyShown)
        {
            TempCanvasGroup.alpha = 1;
            alreadyShown = true;
        }
    }

    public void UpdatePosition()
    {
        Vector2 pos = RectTransformUtility.WorldToScreenPoint(Camera.main, targetPosition);
        TempTransform.position = pos;
    }
}
