//BY FHIZ
//MODIFIED BY DX4D

using UnityEngine;

namespace OpenMMO.Network
{
    public partial class NetworkAuthenticator
    {
        [Header("CLIENT CONNECTION CONFIG")]
        public bool checkApplicationVersion = true;
        [Range(0, byte.MaxValue)] public byte autoConnectDelay = 10;
        //RANDOMIZED DELAY //DEPRECIATED - DX4D
        //[Range(0, 99)] public int connectDelayMin = 4; //REMOVED DX4D
        //[Range(0, 99)] public int connectDelayMax = 8; //REMOVED DX4D
        [Range(1, byte.MaxValue)] public byte connectTimeout = byte.MaxValue;

        [HideInInspector] public byte connectDelay;
    }
}
