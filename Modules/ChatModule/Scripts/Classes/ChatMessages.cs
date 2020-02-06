using Mirror;
using OpenMMO;
using OpenMMO.Chat;

namespace OpenMMO.Chat
{
	
	// -----------------------------------------------------------------------------------
	// MsgChatLoginRequestFromClient
	// @Client -> @Server
	// -----------------------------------------------------------------------------------
    public class MsgChatLoginRequestFromClient : MessageBase
    {
        public string userId = string.Empty;
        public string name = string.Empty;
    }
        
    // -----------------------------------------------------------------------------------
	// MsgChatSendFromClient
	// @Client -> @Server
	// -----------------------------------------------------------------------------------
	public class MsgChatSendFromClient : MessageBase
    {
        public string channelId = string.Empty;
        public string[] chatData;
    }
        
	// -----------------------------------------------------------------------------------
	// MsgChatReceiveFromServer
	// @Server -> @Client
	// -----------------------------------------------------------------------------------
	public class MsgChatReceiveFromServer : MessageBase
    {
        public string channelId = string.Empty;
        public string senderId = string.Empty;
        public string senderName = string.Empty;
        public string message;
    }
    
	// -----------------------------------------------------------------------------------
	// MsgChatLoginSuccessFromServer
	// @Server -> @Client
	// -----------------------------------------------------------------------------------
    public class MsgChatLoginSuccessFromServer : MessageBase
    {
        public string userId = string.Empty;
        public string name = string.Empty;
    }
    
    // -----------------------------------------------------------------------------------
    
}
