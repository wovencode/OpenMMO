//MODIFIED BY DX4D
//SOURCE: "jlt" @ https://forum.unity.com/threads/tab-between-input-fields.263779/page-2
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UINavigation : MonoBehaviour
{
    //#if _CLIENT
    [Header("SELECTABLE UI FIELDS")]
    [Tooltip("A list of objects that can be tabbed through.")]
    [SerializeField] internal List<Selectable> selectables = new List<Selectable>();

    /*[Header("TICK FREQUENCY")]
    [Tooltip("The delay in fixed frames between update ticks.")]
    [SerializeField][Range(1, 60)] int tickDelay = 20;
    */

    [Header("KEY ASSIGNMENT")]
    //NEXT KEY
    [Tooltip("Moves to the next input field in the UI.\ndefault:Tab")]
    [SerializeField] public KeyCode nextFieldKey = KeyCode.Tab;
    //PREVIOUS KEY
    //[Tooltip("Moves to the previous input field in the UI.\ndefault:UpArrow")]
    //[SerializeField] public KeyCode[] previousFieldKey = new KeyCode[] { KeyCode.UpArrow, KeyCode.JoystickButton4 };
    //PREVIOUS SHIFT KEY
    [Tooltip("When holding this key the next field key will select the previous field instead.\ndefault:Shift+Tab")]
    [SerializeField] KeyCode previousFieldShiftKey = KeyCode.LeftShift;
    //ESCAPE
    [Tooltip("Remove focus from all selected UI fields.\ndefault:Escape")]
    [SerializeField] KeyCode escapeFocusKey = KeyCode.Escape;

    [Header("FOCUS SETTINGS")]
    //ALWAYS FOCUS
    [Tooltip("When this is checked, clearing the input with the cancel button etc will return focus to the first selectable element.")]
    [SerializeField] bool alwaysFocused = true;
    //WRAP-AROUND
    [Tooltip("When reaching the end of the list, return to the top.")]
    [SerializeField] bool wrapAroundMode = true;


#if UNITY_EDITOR
    public bool resetValues = true;
    private void OnValidate()
    {
        if (resetValues)
        {
            nextFieldKey = KeyCode.Tab;
            previousFieldShiftKey = KeyCode.LeftShift;
            escapeFocusKey = KeyCode.Escape;
            //tickDelay = 10;
            resetValues = false;
        }
        //if (previousFieldKey == null || previousFieldKey.Length < 1) previousFieldKey = new KeyCode[] { KeyCode.UpArrow, KeyCode.JoystickButton4 };
    }
#endif
    private void Start()
    {
        HandleNavigation(false);
    }
    private void OnEnable()
    {
        //HandleNavigation(false);
        //SelectFirstSelectable();
        //InitialFocus();
        //if (selectables != null && selectables.Count > 0) selectables[0].Select(); //AUTOFOCUS FIRST SELECTABLE
    }

    /*void InitialFocus()
    {
        SelectFirstSelectable();

        GameObject selectedObject = EventSystem.current.currentSelectedGameObject;
        if (selectedObject != null && selectedObject.activeInHierarchy) // Ensure a selection exists and is not an inactive object.
        {
            Selectable currentSelection = selectedObject.GetComponent<Selectable>();
            if (currentSelection != null)
            {
                currentSelection.Select();
            }
        }
    }*/
    
    /*int framecount = 0;
    private void FixedUpdate()
    {
        framecount++;
        if (framecount > 60) framecount = 0;
    }*/
    bool moveNext = false;
    bool moveLast = false;
    bool moveFirst = false;

    private void Update()
    {
        //if (framecount % tickDelay != 0) return;
        if (!gameObject.activeInHierarchy) return;
        
        //NEXT
        if (Input.GetKeyDown(nextFieldKey))
        {
            if (Input.GetKey(previousFieldShiftKey)) { moveLast = true; }
            else { moveNext = true; }
        }

        //PREVIOUS
        //if (Input.GetKeyDown(previousFieldKey))//Input.GetKeyDown(previousFieldKey))
        //{
        //    moveLast = true;
            //HandleNavigation(true);
        //}

        //CANCEL
        if (Input.GetKeyDown(escapeFocusKey))// || Input.GetKeyDown(KeyCode.Return)) //DEPRECIATED - We do not want the enter key to break focus
        {
            moveFirst = true;
        }
        //ALWAYS FOCUSED
        else if (alwaysFocused && EventSystem.current.currentSelectedGameObject == null)
        {
            moveFirst = true;
        }
    }
    private void LateUpdate()
    {
        //if ((framecount % tickDelay) != 0) return;
        if (!gameObject.activeInHierarchy) return;

        if (moveNext) HandleNavigation(false);
        else if (moveLast) HandleNavigation(true);
        if (moveFirst) { SelectFirstSelectable(); }

        moveNext = moveLast = moveFirst = false;
    }

    //ANY KEY IS PRESSED
    bool AnyKeyPressed(KeyCode[] keys)
    {
        foreach (KeyCode key in keys)
        {
            if (Input.GetKeyDown(key)) return true;
        }

        return false;
    }
    //HANDLE NAVIGATION
    private void HandleNavigation(bool isNavigateBackward)
    {
        GameObject selectedObject = EventSystem.current.currentSelectedGameObject;

        if (selectedObject != null && selectedObject.activeInHierarchy) // Ensure a selection exists and is not an inactive object.
        {
            Selectable currentSelection = selectedObject.GetComponent<Selectable>();
            if (currentSelection != null)
            {
                Selectable nextSelection;
                if (isNavigateBackward)
                {
                    if (selectables.IndexOf(currentSelection) == 0)
                    {
                        nextSelection = (wrapAroundMode) ? selectables[selectables.Count - 1] : null;
                    }
                    else
                    {
                        nextSelection = currentSelection.FindSelectableOnUp();
                    }
                }
                else
                {
                    if (selectables.IndexOf(currentSelection) == (selectables.Count - 1))
                    {
                        nextSelection = (wrapAroundMode) ? selectables[0] : null;
                    }
                    else
                    {
                        nextSelection = currentSelection.FindSelectableOnDown();
                    }
                }

                if (nextSelection != null)
                {
                    nextSelection.Select();
                }
            }
            else
            {
                SelectFirstSelectable();
            }
        }
        else
        {
            SelectFirstSelectable();
        }
    }

    private void SelectFirstSelectable()
    {
        if (selectables != null && selectables.Count > 0)
        {
            selectables[0].Select();
        }
    }
    
    /// <summary>
    /// Looks at ordered selectable list to find the selectable we are trying to navigate to and returns it.
    /// </summary>
    private Selectable FindNextSelectable(int currentSelectableIndex, bool isNavigateBackward, bool isWrapAround)
    {
        Selectable nextSelection = null;

        int totalSelectables = selectables.Count;
        if (totalSelectables > 1)
        {
            if (isNavigateBackward)
            {
                if (currentSelectableIndex == 0)
                {
                    nextSelection = (isWrapAround) ? selectables[totalSelectables - 1] : null;
                }
                else
                {
                    nextSelection = selectables[currentSelectableIndex - 1];
                }
            }
            else // Navigate forward.
            {
                if (currentSelectableIndex == (totalSelectables - 1))
                {
                    nextSelection = (isWrapAround) ? selectables[0] : null;
                }
                else
                {
                    nextSelection = selectables[currentSelectableIndex + 1];
                }
            }
        }

        return nextSelection;
    }
//#endif //_CLIENT
}
