//BY DX4D
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using OpenMMO;
using OpenMMO.Debugging;
using Mirror;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace OpenMMO
{
    /// <summary> Holds a list of all zones that are managed by the network server. </summary>
    [CreateAssetMenu(fileName = "New Zone Config", menuName = "OpenMMO - Templates/New Zone Config", order = 999)]
    public partial class ZoneConfigTemplate : ScriptableObject
    {
        [Header("Network Zones")]
        public NetworkZoneTemplate mainZone;
        public List<NetworkZoneTemplate> subZones;

        [Header("Settings")]
        [Tooltip("MainZone data save interval (in seconds)")]
        public float zoneSaveInterval = 60f;
    }
}