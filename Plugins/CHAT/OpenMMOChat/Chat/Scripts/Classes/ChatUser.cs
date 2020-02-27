using Mirror;
using OpenMMO;
using OpenMMO.Chat;

namespace OpenMMO.Chat
{
	
	// -----------------------------------------------------------------------------------
	// ChatUser
	// -----------------------------------------------------------------------------------
    [System.Serializable]
    public class ChatUser
    {
        public NetworkConnection conn;
        public string userId;
        public string name;

        public ChatUser(string userId, string name) : this(null, userId, name)
        {
        }

        public ChatUser(NetworkConnection conn, string userId, string name)
        {
            this.conn = conn;
            this.userId = userId;
            this.name = name;
        }
        
    }
    
    // -----------------------------------------------------------------------------------
    
}
