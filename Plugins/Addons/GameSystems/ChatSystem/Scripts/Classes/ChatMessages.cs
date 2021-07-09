//UPDATED MIRROR VERSION v13 to v42.2.8 BY DX4D
using Mirror;
using OpenMMO;
using OpenMMO.Chat;

namespace OpenMMO.Chat
{

    // -----------------------------------------------------------------------------------
    // MsgChatLoginRequestFromClient
    // @Client -> @Server
    // -----------------------------------------------------------------------------------
    //public class MsgChatLoginRequestFromClient : MessageBase //DEPRECIATED - MIRROR UPDATE - MessageBase Replaced with NetworkMessage - Must be nullable (eg struct not class) - DX4D
    public struct MsgChatLoginRequestFromClient : NetworkMessage
    {
        //public string userId = string.Empty; //REMOVED - MIRROR UPDATE - DX4D
        public string userId; //ADDED - MIRROR UPDATE - DX4D
        //public string name = string.Empty; //REMOVED - MIRROR UPDATE - DX4D
        public string name; //ADDED - MIRROR UPDATE - DX4D
    }

    // -----------------------------------------------------------------------------------
    // MsgChatSendFromClient
    // @Client -> @Server
    // -----------------------------------------------------------------------------------
    //public class MsgChatSendFromClient : MessageBase //DEPRECIATED - MIRROR UPDATE - MessageBase Replaced with NetworkMessage - Must be nullable (eg struct not class) - DX4D
    public struct MsgChatSendFromClient : NetworkMessage
    {
        //public string channelId = string.Empty; //REMOVED - MIRROR UPDATE - DX4D
        public string channelId; //ADDED - MIRROR UPDATE - DX4D
        public string[] chatData;
    }
        
	// -----------------------------------------------------------------------------------
	// MsgChatReceiveFromServer
	// @Server -> @Client
	// -----------------------------------------------------------------------------------
	//public class MsgChatReceiveFromServer : MessageBase //DEPRECIATED - MIRROR UPDATE - MessageBase Replaced with NetworkMessage - Must be nullable (eg struct not class) - DX4D
    public struct MsgChatReceiveFromServer : NetworkMessage
    {
        //public string channelId = string.Empty; //REMOVED - MIRROR UPDATE - DX4D
        public string channelId; //ADDED - MIRROR UPDATE - DX4D
        //public string senderId = string.Empty; //REMOVED - MIRROR UPDATE - DX4D
        public string senderId; //ADDED - MIRROR UPDATE - DX4D
        //public string senderName = string.Empty; //REMOVED - MIRROR UPDATE - DX4D
        public string senderName; //ADDED - MIRROR UPDATE - DX4D
        public string message;
    }
    
	// -----------------------------------------------------------------------------------
	// MsgChatLoginSuccessFromServer
	// @Server -> @Client
	// -----------------------------------------------------------------------------------
    //public class MsgChatLoginSuccessFromServer : MessageBase //DEPRECIATED - MIRROR UPDATE - MessageBase Replaced with NetworkMessage - Must be nullable (eg struct not class) - DX4D
    public struct MsgChatLoginSuccessFromServer : NetworkMessage
    {
        //public string userId = string.Empty; //REMOVED - MIRROR UPDATE - DX4D
        public string userId; //ADDED - MIRROR UPDATE - DX4D
        //public string name = string.Empty; //REMOVED - MIRROR UPDATE - DX4D
        public string name; //ADDED - MIRROR UPDATE - DX4D
    }
    
    // -----------------------------------------------------------------------------------
    
}
