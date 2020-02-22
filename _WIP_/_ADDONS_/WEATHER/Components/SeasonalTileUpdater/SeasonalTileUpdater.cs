//BY DX4D
using UnityEngine;

/// <summary>Updates the materials and model of an object based on the season.</summary>
public class SeasonalTileUpdater : MonoBehaviour
{
#pragma warning disable CS0414
    [Header("UPDATE FREQUENCY")]
    [Tooltip("How many update frames must pass before this component updates again?")]
    [SerializeField] [Range(1, 600)] int tickFrequency = 300; //TICK RATE
#pragma warning restore CS0414

    [Header("WEATHER MANAGER")]
    [SerializeField] WeatherManager _weather;

    //SEASON
    [SerializeField] Season _season;
    public Season season
    {
        get
        {
            return _season = _weather.season;
        }
    }

    //SEASONAL MATERIALS
    [Header("SEASONAL MATERIALS")]
    [SerializeField] public SeasonalTile seasonalTile;

    //MESH RENDERER
    [Header("-component links- (autoloaded)")]
    [SerializeField] new MeshRenderer renderer;


#if _CLIENT

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!_weather) _weather = FindObjectOfType<WeatherManager>();
        if (!_weather)
        {
            //TODO: CREATE WEATHER MANAGER
            GameObject go = new GameObject();
            go.AddComponent<WeatherManager>();
            _weather = go.GetComponent<WeatherManager>();
            Instantiate<GameObject>(go);
            //WeatherManager manager = new WeatherManager();
        }
        UpdateSeason();
    }
#endif

    int frameCount = 0; //FRAME COUNTER
    void FixedUpdate()
    {
        //if (!PlayerComponent.localPlayer) return; //NOT LOGGED IN

        frameCount++; //INCREMENT TICK

        if (frameCount >= tickFrequency) //TICK THIS FRAME?
        {
            frameCount = 0; //RESET THE COUNTER

            UpdateSeason();
        }
    }

    //UPDATE SEASON
    bool seasonLoaded = false;
    private void UpdateSeason()
    {
        if (!seasonalTile) return; //NO TILE DATA

        //LOAD RENDERER
        if (!renderer) renderer = GetComponent<MeshRenderer>();

        //LOAD SEASON
        if (!seasonLoaded)
        {
            _season = _weather.season;
            seasonLoaded = true;
        }
        else
        {
            _weather.SetSeason(_season);
        }
        //if (renderer.materials.Length < 2) { renderer.materials = new Material[2]; }

        switch (_weather.season)
        {
            case Season.Spring:
                {
                    SwapMaterials(seasonalTile.materials.spring);
                    break;
                }
            case Season.Summer:
                {
                    SwapMaterials(seasonalTile.materials.summer);
                    break;
                }
            case Season.Fall:
                {
                    SwapMaterials(seasonalTile.materials.fall);
                    break;
                }
            case Season.Winter:
                {
                    SwapMaterials(seasonalTile.materials.winter);
                    break;
                }
        }
    }

    //SWAP MATERIALS
    /// <summary>Swaps materials on the attached object's MeshRenderer component.</summary>
    /// <param name="materials"></param>
    void SwapMaterials(GroundMaterial materials)
    {
        if (!materials.baseMaterial || !materials.topMaterial) return;
        if (renderer.sharedMaterials.Length >= 2 && renderer.sharedMaterials[0] == materials.baseMaterial && renderer.sharedMaterials[1] == materials.topMaterial) return;

        renderer.sharedMaterials = new Material[2] { materials.baseMaterial, materials.topMaterial };
        //if (materials.baseMaterial) renderer.materials[0] = materials.baseMaterial;
        //if (materials.topMaterial) renderer.materials[1] = materials.topMaterial;
    }
#endif
}