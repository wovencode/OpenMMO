using UnityEngine;
using OpenMMO;
using OpenMMO.Chat;

namespace OpenMMO.Chat
{

	// -----------------------------------------------------------------------------------
	// WhisperChatChannelData
	// -----------------------------------------------------------------------------------
    [System.Serializable]
    public class WhisperChatChannelData : ChatChannelData
    {
        public WhisperChatChannelData(string channelId, string command, string prefixSend, string prefixReceive, Color color) : base(channelId, command, prefixSend, prefixReceive, color)
        {
        }

        public override ChatChannelDataResult DoChatLogic(ChatUser senderUser, string[] chatData)
        {
            if (chatData.Length == 2)
            {
                string targetId = chatData[0];
                string message = chatData[1];
                foreach (ChatUser chatUser in ChatManager.singleton.ChatUsers.Values)
                {
                    if (chatUser.userId.Equals(targetId))
                        return new ChatChannelDataResult(this, message, false, senderUser, chatUser);
                }
            }
            return null;
        }

        public override string GetChannelKeyMessage(ChatMessage message = null)
        {
            string keyMessage = base.GetChannelKeyMessage(message);
            if (message != null)
                keyMessage = " " + message.senderName;
            return keyMessage;
        }

        public override string[] GetChatData(string message)
        {
            if (message.Substring(0, command.Length).Equals(command))
                message = message.Substring(command.Length);

            string[] splitedData = message.Split(' ');
            if (splitedData.Length > 1)
            {
                string targetName = splitedData[0];
                string chatMessage = message.Substring(targetName.Length);
                return new string[2] { targetName, chatMessage };
            }
            return null;
        }
    }
    
    // -----------------------------------------------------------------------------------
    
}
