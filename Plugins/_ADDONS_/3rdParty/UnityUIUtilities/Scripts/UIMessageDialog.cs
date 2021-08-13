using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIMessageDialog : UIBase
{
    public struct Data
    {
        public string title;
        public string content;
        public UnityAction actionYes;
        public UnityAction actionNo;
        public UnityAction actionCancel;

        public Data(string title, string content, UnityAction actionYes = null, UnityAction actionNo = null, UnityAction actionCancel = null)
        {
            this.title = title;
            this.content = content;
            this.actionYes = actionYes;
            this.actionNo = actionNo;
            this.actionCancel = actionCancel;
        }
    }

    public Text textTitle;
    public Text titleContent;
    public UnityAction actionYes;
    public UnityAction actionNo;
    public UnityAction actionCancel;

    public string Title
    {
        get { return textTitle == null ? "" : textTitle.text; }
        set { if (textTitle != null) textTitle.text = value; }
    }

    public string Content
    {
        get { return titleContent == null ? "" : titleContent.text; }
        set { if (titleContent != null) titleContent.text = value; }
    }

    protected override void Awake()
    {
        base.Awake();
        if (textTitle == null) Debug.LogWarning("`Text Title` for " + name + " has not been set");
        if (titleContent == null) Debug.LogWarning("`Text Content` for " + name + " has not been set");
    }

    public void SetData(Data data)
    {
        Title = data.title;
        Content = data.content;
    }

    public void OnClickYes()
    {
        if (actionYes != null)
            actionYes.Invoke();
        Hide();
    }

    public void OnClickNo()
    {
        if (actionNo != null)
            actionNo.Invoke();
        Hide();
    }

    public void OnClickCancel()
    {
        if (actionCancel != null)
            actionCancel.Invoke();
        Hide();
    }
}
