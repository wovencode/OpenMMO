//BY FHIZ
//MODIFIED BY DX4D

using UnityEngine;

namespace OpenMMO.Network
{
    public partial class NetworkAuthenticator
    {
        [Header("CLIENT CONNECTION CONFIG")]
        public bool checkApplicationVersion = true;
        [Range(0, 99)]
        public int connectDelayMin = 4;
        [Range(0, 99)]
        public int connectDelayMax = 8;
        [Range(1, 999)]
        public int connectTimeout = 999;

        [HideInInspector] public int connectDelay;
    }
}
