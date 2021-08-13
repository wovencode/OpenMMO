//BY DX4D
using UnityEngine;
using Mirror;

namespace OpenMMO
{
    [DisallowMultipleComponent]
    public partial class CharacterDescription : NetworkBehaviour
    {
        //ICON
        [SerializeField] Sprite _icon = null;
        public Sprite icon { get { return _icon; } set { _icon = value; } }
    }
}