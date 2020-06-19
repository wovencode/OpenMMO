//BY DX4D
using UnityEngine;
using OpenMMO.UI;
using TMPro;

namespace OpenMMO.Chat
{
    public partial class UIChatSlot : UIButton
    {
#pragma warning disable CS0649
        [Header("TEXT LABEL")]
        [SerializeField] TextMeshProUGUI label;
#pragma warning restore CS0649

        //INIT
        public void Init(ChatMessage message)
        {
            if (label == null || message == null) return;

            label.text = message.GetMessage();
        }
    }
}
