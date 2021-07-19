//BY DX4D

using UnityEngine;

namespace OpenMMO.Zones
{
	public abstract partial class ActionTrigger : MonoBehaviour
    {
        [Header("ATTACHED ACTION")]
        [Tooltip("The Action that is activated when entering this area.")]
        public BaseNetworkAction action;

#if UNITY_EDITOR
        void OnValidate()
        {
            ValidateAction();
            //if (!action) action = Resources.Load<NetworkAction>("NetworkActions/Teleport");// ScriptableObject.CreateInstance<TeleportAction>(); //TODO: This should really be in a resources folder
            //action.targetName = targetLocationMarkerName; //SET TARGET NAME
        }
#endif
        //VALIDATE ACTION - DEFAULTS TO TELEPORT ACTION
        internal void ValidateAction()
        {
            if (!action) action = Resources.Load<BaseNetworkAction>("NetworkActions/Teleport");
        }
        //AWAKE
        private void OnEnable() { ValidateAction(); }

        //ON CLICK CONFIRM @Client
        public virtual void OnClickConfirm() { action.OnConfirmed(); } //TODO: Do we use this anymore?
		//ON TRIGGER ENTER @Client / @Server
		public virtual void OnTriggerEnter(Collider co) { ValidateAction(); action.OnRangeEntered(co); }
        //ON TRIGGER EXIT @Client / @Server
        public virtual void OnTriggerExit(Collider co) { if (action != null) action.OnRangeLeft(co); }
	}
}