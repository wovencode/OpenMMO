using System;
using UnityEngine;
using OpenMMO;
using OpenMMO.Chat;

namespace OpenMMO.Chat
{

	// ===================================================================================
	// ChatMessage
	// ===================================================================================
    [System.Serializable]
    public class ChatMessage
    {
    
        public enum ChatState
        {
            Send,
            Receive
        }
        
        public ChatChannelData channelData;
        public string senderId;
        public string senderName;
        public string message;
        public ChatState chatState;
        public float receiveTime;
		
		// -------------------------------------------------------------------------------
		// ChatMessage (Constructor)
		// -------------------------------------------------------------------------------
        public ChatMessage(ChatChannelData channelData, string senderId, string senderName, string message, ChatState chatState)
        {
            this.channelData = channelData;
            this.senderId = senderId;
            this.senderName = senderName;
            this.message = message;
            this.chatState = chatState;
            this.receiveTime = Time.unscaledTime;
        }
		
		// -------------------------------------------------------------------------------
		// GetMessage
		// -------------------------------------------------------------------------------
        public string GetMessage()
        {
            string prefix = chatState == ChatState.Send ? channelData.prefixSend : channelData.prefixReceive;
            
			if (String.IsNullOrWhiteSpace(prefix) && String.IsNullOrWhiteSpace(senderName))
				return message;
			
            return "<b>" + prefix + senderName + "</b>: " + message;
            
        }
        
        // -------------------------------------------------------------------------------
        
    }
    
    // -----------------------------------------------------------------------------------
    
}
