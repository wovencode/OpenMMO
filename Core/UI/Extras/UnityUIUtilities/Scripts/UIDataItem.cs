using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum UIDataItemClickMode
{
    Default,
    Selection,
    Disable,
}

[System.Serializable]
public class UIDataItemEvent : UnityEvent<UIDataItem> { }

public abstract class UIDataItem : UIBase
{
    // Decoration
    public bool selected;
    protected bool dirtySelected;
    public GameObject selectedObject;
    public GameObject emptyInfoObject;
    // Events
    public UIDataItemClickMode clickMode = UIDataItemClickMode.Default;
    public UIDataItemEvent eventClick;
    public UIDataItemEvent eventSelect;
    public UIDataItemEvent eventDeselect;
    public UIDataItemEvent eventUpdate;
    public abstract object GetData();
    [HideInInspector]
    public UIList list;

    public virtual bool Selected
    {
        get { return selected; }
        set { selected = value; }
    }
}

public abstract class UIDataItem<T> : UIDataItem where T : class
{
    // Data
    public T data;
    protected T dirtyData;

    public bool IsDirty()
    {
        return data != dirtyData;
    }

    protected override void Awake()
    {
        base.Awake();
        if (selectedObject != null)
            selectedObject.SetActive(false);
        if (emptyInfoObject != null)
            emptyInfoObject.SetActive(false);
    }

    protected virtual void Update()
    {
        if (IsDirty())
            UpdateLogic();

        if (Selected != dirtySelected)
        {
            if (selectedObject != null)
                selectedObject.SetActive(Selected);
            dirtySelected = Selected;
        }

        if (emptyInfoObject != null)
            emptyInfoObject.SetActive(IsEmpty());
    }

    private void UpdateLogic()
    {
        Clear();
        UpdateData();
        dirtyData = data;
        eventUpdate.Invoke(this);
    }

    public void ForceUpdate()
    {
        UpdateLogic();
    }

    public virtual void OnClick()
    {
        switch (clickMode)
        {
            case UIDataItemClickMode.Selection:
                if (list == null ||
                    list.limitSelection <= 0 ||
                    list.SelectedAmount < list.limitSelection ||
                    Selected)
                {
                    Selected = !Selected;
                    if (Selected)
                        Select();
                    else
                        Deselect();
                }
                break;
            case UIDataItemClickMode.Default:
                Click();
                break;
        }
    }

    public virtual void Select(bool invokeEvent = true)
    {
        Selected = true;
        if (invokeEvent)
            eventSelect.Invoke(this);
    }

    public virtual void Deselect(bool invokeEvent = true)
    {
        Selected = false;
        if (invokeEvent)
            eventDeselect.Invoke(this);
    }

    public virtual void Click(bool invokeEvent = true)
    {
        Selected = false;
        if (invokeEvent)
            eventClick.Invoke(this);
    }

    public override object GetData()
    {
        return data;
    }

    public void SetData(T newData)
    {
        data = newData;
        ForceUpdate();
    }

    public abstract void UpdateData();
    public abstract void Clear();
    public abstract bool IsEmpty();
}
