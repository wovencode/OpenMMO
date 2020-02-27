//IMPROVEMENTS BY DX4D
using Mirror;
using UnityEngine;
using System.Collections.Generic;

namespace OpenMMO.Chat
{

	// ===================================================================================
	// ChatManager
	// ===================================================================================
	[DisallowMultipleComponent]
    public class ChatManager : MonoBehaviour
    {
        [Header("PROFANITY FILTER")]
        [SerializeField] internal ProfanityFilter profanityFilter;
        internal static string[] WordsToFilter
        {
            get { return singleton.profanityFilter.wordsToFilter; }
        }

        internal static string ProfanityMask
        {
            get { return singleton.profanityFilter.profanityMask; }
        }

        [Header("CHAT CHANNELS")]
        public ChatChannelData defaultChannel = new ChatChannelData("default", "", "", "", Color.white);
        public ChatChannelData[] channelList;
        public delegate void ChatMessageReceiveEvent(ChatManager chatManager, ChatMessage chatMessage);
        public static ChatMessageReceiveEvent onReceiveMessage;

        private Dictionary<string, ChatChannelData> channels;
        public Dictionary<string, ChatChannelData> Channels
        {
            get
            {
                if (channels == null)
                {
                    channels = new Dictionary<string, ChatChannelData>();
                    if (channelList == null || channelList.Length == 0)
                        channelList = new ChatChannelData[1] { defaultChannel };

                    for (int i = 0; i < channelList.Length; ++i)
                    {
                        ChatChannelData channel = channelList[i];
                        if (channel != null && !string.IsNullOrEmpty(channel.channelId) && !channels.ContainsKey(channel.channelId))
                            channels.Add(channel.channelId, channel);
                    }
                }
                return channels;
            }
        }
        private List<ChatMessage> messages;
        public List<ChatMessage> Messages
        {
            get
            {
                if (messages == null)
                    messages = new List<ChatMessage>();
                return messages;
            }
        }
        private Dictionary<NetworkConnection, ChatUser> chatUsers;
        public Dictionary<NetworkConnection, ChatUser> ChatUsers
        {
            get
            {
                if (chatUsers == null)
                    chatUsers = new Dictionary<NetworkConnection, ChatUser>();
                return chatUsers;
            }
        }
        
        public static ChatManager singleton;
        //public NetworkClient client;
        private ChatUser clientChatUser;
        
		// -------------------------------------------------------------------------------
		// Awake
		// -------------------------------------------------------------------------------
        void Awake()
        {
            singleton = this;
            DontDestroyOnLoad(gameObject);
        }

        //VALIDATE
        private void OnValidate()
        {
            if (!profanityFilter) profanityFilter = Resources.Load<ProfanityFilter>("Chat/Filters/DefaultProfanityFilter");
        }

        // ================================ PUBLIC =======================================

        // -------------------------------------------------------------------------------
        // OnLoginPlayer
        // -------------------------------------------------------------------------------
        public void OnLoginPlayer(NetworkConnection conn)
		{
			//client = NetworkClient.singleton; //DEPRECIATED
			
			if (conn.identity.gameObject != null)
			{
				string name = conn.identity.gameObject.name;
				string userId = System.Guid.NewGuid().ToString();
				AddChatUser(new ChatUser(conn, userId, name));
			}
			
		}
		
		// -------------------------------------------------------------------------------
		// OnLogoutPlayer
		// -------------------------------------------------------------------------------
		public void OnLogoutPlayer(NetworkConnection conn)
		{
			RemoveChatUser(conn);
		}
		
		// -------------------------------------------------------------------------------
        public void SetupServerMessages()
        {
            NetworkServer.RegisterHandler<MsgChatSendFromClient>(OnServerChatReceive);
            NetworkServer.RegisterHandler<MsgChatLoginRequestFromClient>(OnServerLoginRequest);
        }
        
		// -------------------------------------------------------------------------------
        public void SetupClientMessages()
        {
            NetworkClient.RegisterHandler<MsgChatReceiveFromServer>(OnClientChatReceive);
            NetworkClient.RegisterHandler<MsgChatLoginSuccessFromServer>(OnClientLoginSuccess);
        }
		
		// ================================ PROTECTED ====================================
		
		// -------------------------------------------------------------------------------
        public void AddChatUser(NetworkConnection conn, string userId, string name)
        {
            AddChatUser(new ChatUser(conn, userId, name));
        }
        
		// -------------------------------------------------------------------------------
        public void AddChatUser(ChatUser user)
        {
            if (user != null && user.conn != null && !ChatUsers.ContainsKey(user.conn))
            {
                ChatUsers.Add(user.conn, user);
                MsgChatLoginSuccessFromServer msg = new MsgChatLoginSuccessFromServer();
                msg.userId = user.userId;
                msg.name = user.name;
                user.conn.Send(msg);
            }
        }
        
		// -------------------------------------------------------------------------------
        protected void RemoveChatUser(NetworkConnection conn)
        {
            if (conn != null && ChatUsers.ContainsKey(conn))
                ChatUsers.Remove(conn);
        }
        
		// -------------------------------------------------------------------------------
        public void ClearChatUser()
        {
            ChatUsers.Clear();
        }
        
		// -------------------------------------------------------------------------------
        public bool ContainsChatUserId(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return false;
            var enumerator = ChatUsers.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (enumerator.Current.Value.userId.Equals(userId))
                    return true;
            }
            return false;
        }
        
		// -------------------------------------------------------------------------------
        public void SetClientChatUser(string userId, string name)
        {
            SetClientChatUser(new ChatUser(NetworkClient.connection, userId, name));
        }
        
		// -------------------------------------------------------------------------------
        public void SetClientChatUser(ChatUser user)
        {
            clientChatUser = user;
        }
        
		// -------------------------------------------------------------------------------
        public void ClearClientChatUser()
        {
            clientChatUser = null;
        }
        
		// -------------------------------------------------------------------------------
        public void ClientChatReceive(ChatChannelDataResult result)
        {
            if (result == null)
            {
                Debug.LogWarning("[Warning] Can not receive null chat result");
                return;
            }

            MsgChatReceiveFromServer msg = new MsgChatReceiveFromServer();
            msg.channelId = result.channel.channelId;
            msg.senderId = result.sender.userId;
            msg.senderName = result.sender.name;
            msg.message = result.message;
            if (result.isBroadcast)
            {
                foreach (ChatUser user in ChatUsers.Values)
                {
                    if (user.conn != null)
                        user.conn.Send(msg);
                }
            }
            else
            {
                if (result.receiver != null && result.receiver.conn != null)
                    result.receiver.conn.Send(msg);
            }
        }
		
		// -------------------------------------------------------------------------------
		// ClientChatSend
		// -------------------------------------------------------------------------------
        public void ClientChatSend(string channelId, string message)
        {
            if (clientChatUser == null || string.IsNullOrEmpty(message))
            {
                Debug.LogWarning("[Warning] Did not login to server can not send chat message");
                return;
            }
            
            // -- Profanity Filter (applied client side already)
            message = profanityFilter.FilterText(message);

            ChatChannelData channel = null;
            if (Channels.TryGetValue(channelId, out channel))
            {
                string[] chatData = channel.GetChatData(message);
                if (chatData != null && chatData.Length > 0)
                {
                    NetworkConnection conn = NetworkClient.connection;
                    MsgChatSendFromClient chatSendMsg = new MsgChatSendFromClient();
                    chatSendMsg.channelId = channelId;
                    chatSendMsg.chatData = chatData;
                    conn.Send(chatSendMsg);
                }
                else
                    Debug.LogWarning("[Warning] Invalid chat data");
            }
            else
                Debug.LogWarning("[Warning] Chat channel (" + channelId + ") not found");
        }
        
        
        // -------------------------------------------------------------------------------
		// LocalChatSend
		// -------------------------------------------------------------------------------
		public void LocalChatSend(string message, string channelId="info")
		{
			
			if (clientChatUser == null || string.IsNullOrEmpty(message))
            {
                Debug.LogWarning("[Warning] Did not login to server can not send chat message");
                return;
            }
			
			ChatChannelData channel = null;
			
            if (Channels.TryGetValue(channelId, out channel))
            {
			Debug.Log(message);
				ChatMessage chatMessage = new ChatMessage(channel, "", "", message, ChatMessage.ChatState.Send);
				
				if (UIWindowChat.singleton)
            		UIWindowChat.singleton.OnReceiveChatMessage(chatMessage);
			}
			
		}
        
		// -------------------------------------------------------------------------------
        public void OnServerChatReceive(NetworkConnection conn, MsgChatSendFromClient msg)
        {
            ChatChannelData channel = defaultChannel;
            if (ChatUsers.ContainsKey(conn))
            {
                ChatUser user = ChatUsers[conn];
                if (Channels.ContainsKey(msg.channelId))
                    channel = Channels[msg.channelId];
                else
                    Debug.LogWarning("[Warning] Chat channel (" + msg.channelId + ") not found");

                if (channel != null)
                    ClientChatReceive(channel.DoChatLogic(user, msg.chatData));
            }
            else
                Debug.LogError("[Error] Invalid chat user " + conn.connectionId);
        }
        
		// -------------------------------------------------------------------------------
        public void OnServerLoginRequest(NetworkConnection conn, MsgChatLoginRequestFromClient msg)
        {
            string userId = msg.userId;

            if (string.IsNullOrEmpty(userId))
                userId = System.Guid.NewGuid().ToString();

            if (string.IsNullOrEmpty(msg.name))
            {
                Debug.LogWarning("[Warning] Chat user " + conn.connectionId + " entered empty name");
                return;
            }

            if (ContainsChatUserId(userId))
            {
                Debug.LogWarning("[Warning] Chat user " + conn.connectionId + " user id " + userId + " already exists");
                return;
            }

            AddChatUser(conn, userId, msg.name);
        }
        
		// -------------------------------------------------------------------------------
		// OnClientChatReceive
		// -------------------------------------------------------------------------------
        public void OnClientChatReceive(NetworkConnection conn, MsgChatReceiveFromServer msg)
        {
            ChatChannelData channel = defaultChannel;
            
            if (Channels.ContainsKey(msg.channelId))
                channel = Channels[msg.channelId];
            else
                Debug.LogWarning("[Warning] Chat channel (" + msg.channelId + ") not found");

            if (channel != null)
            {
            
                ChatMessage.ChatState chatState = ChatMessage.ChatState.Receive;
                
                if (msg.senderId.Equals(clientChatUser.userId))
                    chatState = ChatMessage.ChatState.Send;
                
                ChatMessage chatMessage = new ChatMessage(channel, msg.senderId, msg.senderName, msg.message, chatState);
                Messages.Add(chatMessage);
                
                if (onReceiveMessage != null)
                    onReceiveMessage(this, chatMessage);
                    
                if (UIWindowChat.singleton)
                    UIWindowChat.singleton.OnReceiveChatMessage(chatMessage);
            }
        }
        
		// -------------------------------------------------------------------------------
        public void OnClientLoginSuccess(NetworkConnection conn, MsgChatLoginSuccessFromServer msg)
        {
            if (!string.IsNullOrEmpty(msg.userId))
                SetClientChatUser(msg.userId, msg.name);
        }
        
    }
	
	// ===================================================================================
	
}
