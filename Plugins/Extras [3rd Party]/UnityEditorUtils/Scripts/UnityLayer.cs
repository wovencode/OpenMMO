using UnityEngine;

[System.Serializable]
public class UnityLayer
{
    [SerializeField]
    private int layerIndex = 0;

    public int LayerIndex
    {
        get { return layerIndex; }
    }

    public static implicit operator int(UnityLayer unityLayer)
    {
        return unityLayer.LayerIndex;
    }

    public void Set(int _layerIndex)
    {
        if (_layerIndex > 0 && _layerIndex < 32)
            layerIndex = _layerIndex;
    }

    public int Mask
    {
        get { return 1 << layerIndex; }
    }
}