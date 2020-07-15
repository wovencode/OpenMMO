//BY DX4D
#define CORE_ENABLED //TOGGLE ON/OFF WITH COMMENTS
using UnityEngine;
using UnityEngine.AI;

namespace OpenMMO
{
    //CORE COMPONENTS
#if CORE_ENABLED
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(TargetingSystem))]
    [RequireComponent(typeof(InteractionSystem))]
    [RequireComponent(typeof(CharacterDescription))]
#endif

    public partial class CharacterProfile
    {
#if CORE_ENABLED //BEGIN CORE_ENABLED
        //DEBUG STATEMENTS - (const for performance)
        #region DEBUG STATEMENTS
        //DEBUG
#if UNITY_EDITOR && DEBUG
        internal const string DEBUG_CORE_PLUGINS_LOADED_MESSAGE = "<color=green>CORE PLUGINS LOADED</color>";
        internal const string DEBUG_DESCRIPTION_COMPONENT_ADDED_STATEMENT = "<color=green>LOADED</color>: CHARACTER PROFILE - DESCRIPTION";
        internal const string DEBUG_TARGET_COMPONENT_ADDED_STATEMENT = "<color=green>LOADED</color>: CHARACTER PROFILE - TARGETING SYSTEM";
        internal const string DEBUG_INTERACTION_COMPONENT_ADDED_STATEMENT = "<color=green>LOADED</color>: CHARACTER PROFILE - INTERACTION SYSTEM";
        internal const string DEBUG_AGENT_COMPONENT_ADDED_STATEMENT = "<color=green>LOADED</color>: CHARACTER PROFILE - NAVMESH AGENT";
#endif
        #endregion

        [Hook("OnValidate")]
        internal virtual void LoadCoreComponents()
        {
            #region DEBUG - INIT
            //DEBUG
#if UNITY_EDITOR && DEBUG
            System.Text.StringBuilder debugLog = new System.Text.StringBuilder();
            int loadCounter = 0;
#endif
            #endregion

            //CORE COMPONENT CACHE - AUTOLOAD
            if (!_description)
            {
                _description = GetComponent<CharacterDescription>();
                #region DEBUG
                //DEBUG
#if UNITY_EDITOR && DEBUG
                if (_description)
                {
                    debugLog.AppendLine(DEBUG_DESCRIPTION_COMPONENT_ADDED_STATEMENT);
                    loadCounter++;
                }
#endif
                #endregion
            }
            if (!_target)
            {
                _target = GetComponent<TargetingSystem>();
                #region DEBUG
                //DEBUG
#if UNITY_EDITOR && DEBUG
                if (_target)
                {
                    debugLog.AppendLine(DEBUG_TARGET_COMPONENT_ADDED_STATEMENT);
                    loadCounter++;
                }
#endif
                #endregion
            }
            if (!_interaction)
            {
                _interaction = GetComponent<InteractionSystem>();
                #region DEBUG
                //DEBUG
#if UNITY_EDITOR && DEBUG
                if (_interaction)
                {
                    debugLog.AppendLine(DEBUG_INTERACTION_COMPONENT_ADDED_STATEMENT);
                    loadCounter++;
                }
#endif
                #endregion
            }
            if (!_agent)
            {
                _agent = GetComponent<NavMeshAgent>();
                #region DEBUG
                //DEBUG
#if UNITY_EDITOR && DEBUG
                if (_agent)
                {
                    debugLog.AppendLine(DEBUG_AGENT_COMPONENT_ADDED_STATEMENT);
                    loadCounter++;
                }
#endif
                #endregion
            }
            #region WRITE DEBUG LOG
            //DEBUG
#if UNITY_EDITOR && DEBUG
            if (debugLog.ToString() != string.Empty) Debug.Log(DEBUG_CORE_PLUGINS_LOADED_MESSAGE + " - " + name + "\n" + " linked " + loadCounter + " components" + "\n" + debugLog.ToString());
#endif
            #endregion
        }
#endif //END CORE ENABLED


#if CORE_ENABLED //BEGIN CORE ENABLED
        [Header("CORE COMPONENTS")]
        //NAVMESH AGENT
        [SerializeField] NavMeshAgent _agent = null;
        public NavMeshAgent agent
        {
            get
            {
                if (!_agent) _agent = GetComponent<NavMeshAgent>();
                return _agent;
            }
        }
        //TARGET
        [SerializeField] TargetingSystem _target = null;
        public TargetingSystem targets
        {
            get
            {
                if (!_target) _target = GetComponent<TargetingSystem>();
                return _target;
            }
        }
        //TARGET PROFILE
        internal CharacterProfile targetProfile
        {
            get { return ((targets.target) ? targets.target.profile : null); }
        }
        //INTERACTION
        [SerializeField] InteractionSystem _interaction = null;
        public InteractionSystem interaction
        {
            get
            {
                if (!_interaction) _interaction = GetComponent<InteractionSystem>();
                return _interaction;
            }
        }
        //DESCRIPTION
        [SerializeField] CharacterDescription _description = null;
        internal CharacterDescription description
        {
            get
            {
                if (!_description) _description = GetComponent<CharacterDescription>();
                return _description;
            }
        }
#endif //END CORE_ENABLED
    }
}