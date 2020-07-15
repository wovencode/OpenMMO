//BY DX4D
using UnityEngine;
using Mirror;

namespace OpenMMO
{
    public partial class TargetingSystem : NetworkBehaviour
    {
        [SerializeField] CharacterProfile _profile;
        public CharacterProfile profile
        {
            get
            {
                if (!_profile) _profile = GetComponent<CharacterProfile>();
                return _profile;
            }
        }
    }
}