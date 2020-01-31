using UnityEngine;

[System.Serializable]
public class UnityTag
{
    [SerializeField]
    private string tag = "";

    public string Tag
    {
        get { return tag; }
    }

    public static implicit operator string(UnityTag unityTag)
    {
        return unityTag.Tag;
    }

    public void Set(string _tag)
    {
        tag = _tag;
    }
}