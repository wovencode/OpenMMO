//MODIFIED BY DX4D
//SOURCE: "jlt" @ https://forum.unity.com/threads/tab-between-input-fields.263779/page-2
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TabThroughUI : MonoBehaviour
{
    [Header("SELECTABLE UI FIELDS")]
    [Tooltip("A list of objects that can be tabbed through.")]
    [SerializeField] internal List<Selectable> selectables = new List<Selectable>();

    [Header("KEY ASSIGNMENT")]
    //NEXT KEY
    [Tooltip("Moves to the next input field in the UI.\ndefault:Tab")]
    [SerializeField] KeyCode nextFieldKey = KeyCode.DownArrow | KeyCode.Tab | KeyCode.JoystickButton5;
    //PREVIOUS KEY
    [Tooltip("Moves to the previous input field in the UI.\ndefault:UpArrow")]
    [SerializeField] KeyCode previousFieldKey = KeyCode.UpArrow | KeyCode.JoystickButton4;
    //PREVIOUS SHIFT KEY
    [Tooltip("When holding this key the next field key will select the previous field instead.\ndefault:Shift+Tab")]
    [SerializeField] KeyCode previousFieldShiftKey = KeyCode.LeftShift;
    //ESCAPE
    [Tooltip("Remove focus from all selected UI fields.\ndefault:Escape")]
    [SerializeField] KeyCode escapeFocusKey = KeyCode.Escape | KeyCode.JoystickButton1;

    [Header("FOCUS SETTINGS")]
    //ALWAYS FOCUS
    [Tooltip("When this is checked, clearing the input with the cancel button etc will return focus to the first selectable element.")]
    [SerializeField] bool alwaysFocused = true;

    bool AnyKeyPressed(KeyCode[] keys)
    {
        foreach (KeyCode key in keys)
        {
            if (Input.GetKeyDown(key)) return true;
        }

        return false;
    }

    private void Start()
    {
        SelectFirstSelectable();
        //if (selectables != null && selectables.Count > 0) selectables[0].Select(); //AUTOFOCUS FIRST SELECTABLE
    }
    private void Update()
    {
        if (!gameObject.activeInHierarchy) return;

        //NEXT
        if (Input.GetKeyDown(nextFieldKey))
        {
            // Navigate backward when holding shift, else navigate forward.
            this.HandleHotkeySelect(Input.GetKey(previousFieldShiftKey), true);
        }
        //PREVIOUS
        else if (Input.GetKeyDown(previousFieldKey))
        {
            this.HandleHotkeySelect(true, true);
        }
        //CANCEL
        if (Input.GetKeyDown(escapeFocusKey))// || Input.GetKeyDown(KeyCode.Return)) //DEPRECIATED - We do not want the enter key to break focus
        {
            EventSystem.current.SetSelectedGameObject(null, null);
        }

        if (alwaysFocused && EventSystem.current.currentSelectedGameObject == null)
        {
            SelectFirstSelectable();
        }
    }

    private void HandleHotkeySelect(bool isNavigateBackward, bool isWrapAround)
    {
        GameObject selectedObject = EventSystem.current.currentSelectedGameObject;
        if (selectedObject != null && selectedObject.activeInHierarchy) // Ensure a selection exists and is not an inactive object.
        {
            Selectable currentSelection = selectedObject.GetComponent<Selectable>();
            if (currentSelection != null)
            {
                Selectable nextSelection = this.FindNextSelectable(
                    selectables.IndexOf(currentSelection), isNavigateBackward, isWrapAround);
                if (nextSelection != null)
                {
                    nextSelection.Select();
                }
            }
            else
            {
                this.SelectFirstSelectable();
            }
        }
        else
        {
            this.SelectFirstSelectable();
        }
    }

    private void SelectFirstSelectable()
    {
        if (selectables != null && selectables.Count > 0)
        {
            Selectable firstSelectable = selectables[0];
            firstSelectable.Select();
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

}
