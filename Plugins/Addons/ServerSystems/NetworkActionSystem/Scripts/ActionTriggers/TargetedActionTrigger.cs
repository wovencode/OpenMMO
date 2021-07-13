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
	public abstract partial class TargetedActionTrigger : ActionTrigger
    {
        [Header("TARGET")]
        [Tooltip("The name of the target."
             + "\nNOTE: Target must be in the same scene and have a Location Marker component attached")]
        public string targetLocationMarkerName;

        //[Header("ATTACHED ACTION")]
        //[Tooltip("The Action that is activated when entering this area.")]
        //public NetworkAction action;

#if UNITY_EDITOR
        void OnValidate()
        {
            ValidateAction();
            //if (!action) action = ScriptableObject.CreateInstance<TeleportAction>(); //TODO: This should really be in a resources folder
            //action.targetName = targetLocationMarkerName; //SET TARGET NAME
        }
#endif
        void SetTargetName()
        {
            action.targetName = targetLocationMarkerName; //SET TARGET NAME
        }

        // -------------------------------------------------------------------------------
        // OnTriggerEnter
        // @Client / @Server
        // -------------------------------------------------------------------------------
        public override void OnTriggerEnter(Collider co)
        {
            ValidateAction();
            SetTargetName();

            action.OnRangeEntered(co);
        }

        // @Client
        //public override void OnClickConfirm() { action.OnConfirmed(); }

        // -------------------------------------------------------------------------------
        // OnTriggerEnter
        // @Client / @Server
        // -------------------------------------------------------------------------------
        //public override void OnTriggerEnter(Collider co) { action.OnRangeEntered(co); }
        // -------------------------------------------------------------------------------
        // OnTriggerExit
        // @Client / @Server
        // -------------------------------------------------------------------------------
        //public override void OnTriggerExit(Collider co) { action.OnRangeLeft(co); }

        // -------------------------------------------------------------------------------

    }

}

// =======================================================================================