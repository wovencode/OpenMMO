//BY DX4D
using UnityEngine;
using OpenMMO.UI;
using TMPro;

namespace OpenMMO.Chat
{
    public partial class UIChatSlot : UIButton
    {
        [Header("TEXT LABEL")]
        [SerializeField] TextMeshProUGUI label;
        
        //INIT
        public void Init(ChatMessage message)
        {
            label.text = message.GetMessage();

        }
    }
}
