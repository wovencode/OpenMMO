using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(MobileSwipeArea))]
[RequireComponent(typeof(MobilePinchArea))]
public class MobileSwipeAndPinchArea : MobileInputComponent, IPointerDownHandler, IPointerUpHandler
{
    public MobileSwipeArea CacheMobileSwipeArea { get; private set; }
    public MobilePinchArea CacheMobilePinchArea { get; private set; }

    private int pointerCount = 0;

    private void Awake()
    {
        CacheMobileSwipeArea = GetComponent<MobileSwipeArea>();
        CacheMobilePinchArea = GetComponent<MobilePinchArea>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        ++pointerCount;
        if (pointerCount == 2)
            CacheMobilePinchArea.OnPointerDown(eventData);
        if (pointerCount == 1)
        {
            CacheMobilePinchArea.OnPointerDown(eventData);
            CacheMobileSwipeArea.OnPointerDown(eventData);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        --pointerCount;
        if (pointerCount >= 1)
            CacheMobilePinchArea.OnPointerUp(eventData);
        if (pointerCount == 0)
            CacheMobileSwipeArea.OnPointerUp(eventData);
    }

    void Update()
    {
        if (pointerCount > 1)
        {
            CacheMobilePinchArea.enabled = true;
            CacheMobileSwipeArea.enabled = false;
        }
        else if (pointerCount > 0)
        {
            CacheMobilePinchArea.enabled = false;
            CacheMobileSwipeArea.enabled = true;
        }
        else
        {
            CacheMobilePinchArea.enabled = false;
            CacheMobileSwipeArea.enabled = false;
        }
    }
}
