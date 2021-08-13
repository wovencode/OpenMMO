//BY DX4D
using UnityEngine;

namespace OpenMMO
{
    [RequireComponent(typeof(PlayerAccount))]
    [RequireComponent(typeof(CharacterProfile))]

    [RequireComponent(typeof(DisallowMultipleComponent))]
    public class PlayableCharacter : MonoBehaviour
    {
        [SerializeField] CharacterProfile _profile = null;
        public CharacterProfile profile { get { return _profile; } }
        [SerializeField] PlayerAccount _account = null;
        public PlayerAccount account { get { return _account; } }

        private void OnValidate()
        {
            if (!_profile) _profile = GetComponent<CharacterProfile>();
            if (!_account) _account = GetComponent<PlayerAccount>();
        }
    }
}