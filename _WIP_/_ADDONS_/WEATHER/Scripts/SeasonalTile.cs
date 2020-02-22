using UnityEngine;

[CreateAssetMenu(menuName = "OpenMMO/Worldbuilding/Seasonal Tile")]
public class SeasonalTile : ScriptableObject
{
    [Header("SEASONAL MATERIALS")]
    public SeasonalGroundMaterials materials = new SeasonalGroundMaterials(); //SEASONAL MATERIALS
}