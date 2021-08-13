//BY DX4D
using UnityEngine;

namespace OpenMMO
{
    [RequireComponent(typeof(CharacterProfile))]

    [RequireComponent(typeof(DisallowMultipleComponent))]
    public class NonPlayableCharacter : MonoBehaviour
    {
        [SerializeField] CharacterProfile _profile = null;
        public CharacterProfile profile { get { return _profile; } }

        private void OnValidate()
        {
            if (!_profile) _profile = GetComponent<CharacterProfile>();
        }
    }
}