//BY DX4D
using Mirror;
using UnityEngine;

namespace OpenMMO
{
    [UnityEngine.DisallowMultipleComponent] //ONE PER GAME OBJECT

    /// <summary>The Character Profile holds each of the stats of a character all in one place.</summary>
    public partial class CharacterProfile : NetworkBehaviour
    {
        [Header("CHARACTER LEVEL")]
        public ushort level = 1;
        
        private void OnValidate()
        {
            Hook.HookMethod(this, nameof(OnValidate)); //HOOK USAGE: [Hook("OnValidate")]
        }
    }
}
