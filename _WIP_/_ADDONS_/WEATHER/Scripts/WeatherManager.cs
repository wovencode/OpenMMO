//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class WeatherManager : NetworkBehaviour
{
    [SerializeField] Season _season = Season.Summer;
    public Season season
    {
        get { return _season; }
    }
    /// <summary>
    /// Changes the season - returns true if the season changed.
    /// </summary>
    /// <returns>Season Changed?</returns>
    public bool SetSeason(Season season)
    {
        if (season == _season) return false;
        _season = season;

        #region DEBUG (editor only)
#if UNITY_EDITOR
        Debug.Log("[SEASON CHANGED] - " + season);
#endif
        #endregion

        return true;
    }
}
