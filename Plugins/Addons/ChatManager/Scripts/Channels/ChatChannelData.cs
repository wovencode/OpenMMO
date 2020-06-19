using UnityEngine;
using OpenMMO;
using OpenMMO.Chat;

namespace OpenMMO.Chat
{

	// -----------------------------------------------------------------------------------
	// ChatChannelData
	// -----------------------------------------------------------------------------------
    [System.Serializable]
    public class ChatChannelData
    {
        public string channelId;
        public string command;
        public string prefixSend;
        public string prefixReceive;
        public Color color;

        public ChatChannelData(string channelId, string command, string prefixSend, string prefixReceive, Color color)
        {
            this.channelId = channelId;
            this.command = command;
            this.prefixSend = prefixSend;
            this.prefixReceive = prefixReceive;
            this.color = color;
        }

        public virtual ChatChannelDataResult DoChatLogic(ChatUser senderUser, string[] chatData)
        {
            string message = "";
            foreach (string msg in chatData)
                message += msg;
            return new ChatChannelDataResult(this, message, true, senderUser);
        }

        public virtual string GetChannelKeyMessage(ChatMessage message = null)
        {
            string keyMessage = "";
            if (!string.IsNullOrEmpty(command))
                keyMessage = command;
            return keyMessage;
        }

        public virtual string[] GetChatData(string message)
        {
            string[] chatData = new string[1] { message };
            return chatData;
        }
    }
    
    // -----------------------------------------------------------------------------------
    
}
