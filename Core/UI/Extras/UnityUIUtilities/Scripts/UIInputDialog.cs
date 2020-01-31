using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInputDialog : UIMessageDialog
{
    public InputField inputContent;
    public Text inputPlaceHolderText;
    public string defaultPlaceHolder = "Enter text...";
    public int defaultCharacterLimit = 0;
    public InputField.CharacterValidation defaultCharacterValidation = InputField.CharacterValidation.None;
    public InputField.ContentType defaultContentType = InputField.ContentType.Standard;
    public InputField.InputType defaultInputType = InputField.InputType.Standard;
    public InputField.LineType defaultLineType = InputField.LineType.SingleLine;

    public string InputContent
    {
        get { return inputContent == null ? "" : inputContent.text; }
        set { if (inputContent != null) inputContent.text = value; }
    }

    public string InputPlaceHolder
    {
        get { return inputPlaceHolderText == null ? "" : inputPlaceHolderText.text; }
        set { if (inputPlaceHolderText != null) inputPlaceHolderText.text = value; }
    }

    public int InputCharacterLimit
    {
        get { return inputContent == null ? 0 : inputContent.characterLimit; }
        set { if (inputContent != null) inputContent.characterLimit = value; }
    }

    public InputField.CharacterValidation InputCharacterValidation
    {
        get { return inputContent == null ? InputField.CharacterValidation.None : inputContent.characterValidation; }
        set { if (inputContent != null) inputContent.characterValidation = value; }
    }

    public InputField.ContentType InputContentType
    {
        get { return inputContent == null ? InputField.ContentType.Standard : inputContent.contentType; }
        set { if (inputContent != null) inputContent.contentType = value; }
    }

    public InputField.InputType InputInputType
    { 
        get { return inputContent == null ? InputField.InputType.Standard : inputContent.inputType; }
        set { if (inputContent != null) inputContent.inputType = value; }
    }

    public InputField.LineType InputLineType
    {
        get { return inputContent == null ? InputField.LineType.SingleLine : inputContent.lineType; }
        set { if (inputContent != null) inputContent.lineType = value; }
    }

    public override void Hide()
    {
        base.Hide();
        InputContent = "";
    }

    public void SetInputPropertiesToDefault()
    {
        InputPlaceHolder = defaultPlaceHolder;
        InputCharacterLimit = defaultCharacterLimit;
        InputCharacterValidation = defaultCharacterValidation;
        InputContentType = defaultContentType;
        InputInputType = defaultInputType;
        InputLineType = defaultLineType;
    }
}
