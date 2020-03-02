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
    [Tooltip("Moves to the next input field in the UI.\ndefault:Tab")]
    [SerializeField] KeyCode nextFieldKey = KeyCode.Tab;
    //TODO: More Keys
    [Tooltip("When holding this key the next field key will select the previous field instead.\ndefault:Shift+Tab")]
    [SerializeField] KeyCode previousFieldKey = KeyCode.LeftShift;
    //TODO: More Keys
    [Tooltip("Remove focus from all selected UI fields.\ndefault:Escape")]
    [SerializeField] KeyCode escapeFocusKey = KeyCode.Escape;
    //TODO: More Keys

    private void Start()
    {
        if (selectables != null && selectables.Count > 0) selectables[0].Select(); //AUTOFOCUS FIRST SELECTABLE
    }
    private void Update()
    {
        if (!gameObject.activeInHierarchy) return;

        if (Input.GetKeyDown(nextFieldKey))
        {
            // Navigate backward when holding shift, else navigate forward.
            this.HandleHotkeySelect(Input.GetKey(previousFieldKey), true);
        }
        if (Input.GetKeyDown(escapeFocusKey) || Input.GetKeyDown(KeyCode.Return)) //DEPRECIATED - We do not want the enter key to break focus
        {
            EventSystem.current.SetSelectedGameObject(null, null);
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
