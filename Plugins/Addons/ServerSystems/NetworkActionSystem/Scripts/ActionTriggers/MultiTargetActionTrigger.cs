//By Fhiz
//MODIFIED BY DX4D

using UnityEngine;
using OpenMMO.UI;
//using OpenMMO.Chat; //DEPRECIATED - Creates a Depencency With the Chat - This completely breaks the potential modularity of the Chat Plugin.

namespace OpenMMO.Zones
{

	// ===================================================================================
	// BasePortal
	// ===================================================================================
	public abstract partial class MultiTargetActionTrigger : ActionTrigger
    {
        [Header("TELEPORT TO RANDOM MARKER")]
        [Tooltip("The Location Markers in the same scene to teleport to")]
        public string[] targetNames;
        //[Header("ATTACHED ACTION")]
        //[Tooltip("The Action that is activated when entering this area.")]
        //public NetworkAction action;

#if UNITY_EDITOR
        void OnValidate()
        {
            ValidateAction();
            //if (!action) action = ScriptableObject.CreateInstance<TeleportAction>(); //TODO: This should really be in a resources folder
        }
#endif
        void SetRandomTarget()
        {
            if (targetNames.Length <= 0) return;
            targetNames.Shuffle<string>();
            action.targetName = targetNames[0]; //SET TARGET NAME
        }
        // @Client
        //public override void OnClickConfirm() { action.OnConfirmed(); }
        // @Client / @Server
        public override void OnTriggerEnter(Collider co) { SetRandomTarget(); base.OnTriggerEnter(co); }
        // @Client / @Server
        public override void OnTriggerExit(Collider co) { base.OnTriggerExit(co); }
    }

}

// =======================================================================================