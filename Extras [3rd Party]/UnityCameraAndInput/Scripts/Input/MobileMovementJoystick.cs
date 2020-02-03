using UnityEngine;
using UnityEngine.EventSystems;

public class MobileMovementJoystick : MobileInputComponent, IPointerDownHandler, IPointerUpHandler
{
    public int movementRange = 150;
    public bool useAxisX = true;
    public bool useAxisY = true;
    public string axisXName = "Horizontal";
    public string axisYName = "Vertical";
    public bool fixControllerPosition;
    [SerializeField]
    private bool interactable = true;
    [SerializeField]
    private bool setAsLastSiblingOnDrag;
    [SerializeField]
    private bool hideWhileIdle;
    [Tooltip("Container which showing as area that able to control movement")]
    public RectTransform movementBackground;
    [Tooltip("This is the button to control movement")]
    public RectTransform movementController;

    public bool Interactable
    {
        get { return interactable; }
        set { interactable = value; }
    }

    public bool SetAsLastSiblingOnDrag
    {
        get { return setAsLastSiblingOnDrag; }
        set { setAsLastSiblingOnDrag = value; }
    }

    public bool HideWhileIdle
    {
        get { return hideWhileIdle; }
        set { hideWhileIdle = value; }
    }

    public bool IsDragging
    {
        get { return isDragging; }
    }

    public Vector2 CurrentPosition
    {
        get { return currentPosition; }
    }
    
    private Vector3 backgroundOffset;
    private Vector3 defaultControllerLocalPosition;
    private Vector2 startDragPosition;
    private Vector2 startDragLocalPosition;
    private int defaultSiblingIndex;
    private int pointerId;
    private int correctPointerId;
    private bool isDragging;
    private Vector2 currentPosition;
    private CanvasGroup canvasGroup;
    private float defaultCanvasGroupAlpha;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
            defaultCanvasGroupAlpha = 1f;
        }
        else
        {
            defaultCanvasGroupAlpha = canvasGroup.alpha;
        }
        if (movementBackground != null)
        {
            backgroundOffset = movementBackground.position - movementController.position;
        }
        defaultControllerLocalPosition = movementController.localPosition;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!Interactable || isDragging)
            return;

        pointerId = eventData.pointerId;
        if (fixControllerPosition)
            movementController.localPosition = defaultControllerLocalPosition;
        else
            movementController.position = GetPointerPosition(eventData.pointerId);
        if (SetAsLastSiblingOnDrag)
        {
            defaultSiblingIndex = transform.GetSiblingIndex();
            transform.SetAsLastSibling();
        }
        if (movementBackground != null)
            movementBackground.position = backgroundOffset + movementController.position;
        currentPosition = startDragPosition = movementController.position;
        startDragLocalPosition = movementController.localPosition;
        UpdateVirtualAxes(Vector3.zero);
        isDragging = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (SetAsLastSiblingOnDrag)
            transform.SetSiblingIndex(defaultSiblingIndex);
        movementController.localPosition = defaultControllerLocalPosition;
        if (movementBackground != null)
            movementBackground.position = backgroundOffset + movementController.position;
        isDragging = false;
    }

    private void Update()
    {
        if (!isDragging)
        {
            canvasGroup.alpha = hideWhileIdle ? 0f : defaultCanvasGroupAlpha;
            UpdateVirtualAxes(Vector3.zero);
            return;
        }

        canvasGroup.alpha = defaultCanvasGroupAlpha;

        Vector2 newOffset = Vector2.zero;

        correctPointerId = pointerId;
        if (correctPointerId > Input.touchCount - 1)
            correctPointerId = Input.touchCount - 1;

        currentPosition = GetPointerPosition(correctPointerId);

        Vector2 allowedOffset = currentPosition - startDragPosition;
        allowedOffset = Vector2.ClampMagnitude(allowedOffset, movementRange);

        if (useAxisX)
            newOffset.x = allowedOffset.x;

        if (useAxisY)
            newOffset.y = allowedOffset.y;
        
        movementController.localPosition = startDragLocalPosition + newOffset;
        // Update virtual axes
        UpdateVirtualAxes((startDragPosition - (startDragPosition + newOffset)) / movementRange * -1);
    }

    public void UpdateVirtualAxes(Vector2 value)
    {
        if (useAxisX)
            InputManager.SetAxis(axisXName, value.x);

        if (useAxisY)
            InputManager.SetAxis(axisYName, value.y);
    }
}
