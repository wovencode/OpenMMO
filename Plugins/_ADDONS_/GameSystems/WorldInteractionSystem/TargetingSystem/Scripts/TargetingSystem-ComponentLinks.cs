//BY DX4D
#define CORE_ENABLED //TOGGLE ON/OFF WITH //
using UnityEngine;
using Mirror;

namespace OpenMMO
{
    //CORE COMPONENTS
#if CORE_ENABLED
    [RequireComponent(typeof(CharacterProfile))]
#endif
    public partial class TargetingSystem : NetworkBehaviour
    {
        #region DEBUG STATEMENTS
#if CORE_ENABLED //BEGIN CORE_ENABLED
        //DEBUG STATEMENTS - (const for performance)
#if UNITY_EDITOR && DEBUG
        internal const string DEBUG_TARGETING_PLUGINS_LOADED_MESSAGE = "<color=green>TARGETING SYSTEM PLUGINS LOADED</color>";
        internal const string DEBUG_DESCRIPTION_COMPONENT_ADDED_STATEMENT = "<color=green>LOADED</color>: CHARACTER PROFILE - PROFILE";
#endif
#endif //END CORE_ENABLED
        #endregion //END DEBUG
        [SerializeField] CharacterProfile _profile;
        public CharacterProfile profile
        {
            get
            {
                if (!_profile)
                {
                    _profile = GetComponent<CharacterProfile>();
                    #region DEBUG
    #if UNITY_EDITOR && DEBUG
                    if (_profile)
                    {
                        Debug.Log(DEBUG_TARGETING_PLUGINS_LOADED_MESSAGE + "\n" +DEBUG_DESCRIPTION_COMPONENT_ADDED_STATEMENT);
                    }
    #endif
                    #endregion //DEBUG
                }
                return _profile;
            }
        }
    }
}