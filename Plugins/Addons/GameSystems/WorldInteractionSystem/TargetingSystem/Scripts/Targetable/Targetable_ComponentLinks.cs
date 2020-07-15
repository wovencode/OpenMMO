//BY DX4D
using UnityEngine;

namespace OpenMMO
{
    public partial class Targetable
    {
        #region COMPONENT LINKS -autolinked-
        [Header("COMPONENT LINKS -autolinked-")]

        //LOCAL PLAYER
        [SerializeField, ReadOnly] GameObject _player = null;
        internal GameObject player { get { if (!_player) _player = Local.player; return _player; } }

        //PLAYER PROFILE
        [SerializeField, ReadOnly] CharacterProfile _playerProfile = null;
        internal CharacterProfile profile
        {
            get
            {
                if (!_playerProfile && Local.player)
                {
                    _playerProfile = Local.player.GetComponent<CharacterProfile>();
                }
                return _playerProfile;
            }
        }

        //PLAYER ACCOUNT
        [SerializeField, ReadOnly] PlayerAccount _account = null;
        internal PlayerAccount account
        {
            get
            {
                if (!_account && player)
                {
                    _account = player.GetComponent<PlayerAccount>();
                }
                return _account;
            }
        }
        #endregion

        //VALIDATE - Auto-Loads Components
        private void OnValidate()
        {
            if (!_player) _player = Local.player;
            if (!_account) { _account = (player) ? player.GetComponent<PlayerAccount>() : null; }
            if (!_playerProfile && Local.player) { _playerProfile = (player) ? player.GetComponent<CharacterProfile>() : null; }
        }
    }
}