//BY DX4D

//NOTE: TO CREATE A NEW NETWORK MESSAGE HANDLER, COPY THIS SCRIPT AND RENAME IT

using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace OpenMMO.Network
{
    [CreateAssetMenu( menuName = "OpenMMO/Network/Message Handlers/"
        + nameof(BaseNetworkMessageHandler) )] // <- MUST MATCH THIS CLASS' NAME
    public class BaseNetworkMessageHandler : ScriptableNetworkMessageHandler
    {
        #region EDITOR ONLY DESCRIPTION
#if UNITY_EDITOR
        public string description => _description; //NOTE: This is just here to prevent a warning.
        [Tooltip("A description of this message handler that only exists in the editor")]
        [SerializeField][TextArea(minLines:2, maxLines:5)] string _description =
            ""; // <- EDIT THE DESCRIPTION HERE
#endif
        #endregion

        //@CLIENT@SERVER
        //SETTABLE NETWORK ACTION
        [SerializeField] NetworkAction _networkAction = NetworkAction.None;
        public override NetworkAction networkAction => _networkAction;
        //NOTE: --OPTIONAL SECURITY-- (DO BOTH)
        //COMMENT OUT THE ABOVE 2 LINES FOR ADDED SECURITY <- (AT THE COST OF VERSATILITY)
        //UNCOMMENT THE NEXT LINE FOR ADDED SECURITY <- (MUST DO BOTH)
        //public override NetworkAction networkAction => NetworkAction.Authenticate;

        //@CLIENT
        [Client] public override void HandleServerResponse(NetworkConnection conn, ServerResponse response)
        {
#if _CLIENT
            if (!IsValidAction(response.action))
            {
                Debug.Log("[CLIENT ISSUE] - "
                    + "Invalid Response sent by @" + conn.address);
                return; //VALIDATION <- //ESSENTIAL Be sure to include validation for added security
            }
            throw new System.NotImplementedException("[" + nameof(BaseNetworkMessageHandler) + " ISSUE] - "
                + "You must override " + nameof(HandleServerResponse) + " when creating a Network Message Handler.");
#endif
        }
        //@SERVER
        [Server] public override void HandleClientRequest(NetworkConnection conn, ClientRequest request)
        {
#if _SERVER
            if (!IsValidAction(request.action))
            {
                Debug.Log("[SERVER ISSUE] - "
                    + "Invalid Request sent by @" + conn.address);
                return; //VALIDATION <- //ESSENTIAL Be sure to include validation for added security
            }
            throw new System.NotImplementedException("[" + nameof(BaseNetworkMessageHandler) + " ISSUE] - "
                + "You must override " + nameof(HandleClientRequest) + " when creating a Network Message Handler.");
#endif
        }
    }
}
