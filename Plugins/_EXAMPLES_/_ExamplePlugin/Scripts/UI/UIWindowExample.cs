//BY DX4D

using UnityEngine;
using UnityEngine.UI;

namespace OpenMMO.UI
{
    [DisallowMultipleComponent]
    public partial class UIWindowExample : UIRoot
    {
        [Header("EXAMPLE TESTING BUTTON")]
        public Button testExampleButton;
        public Text testExampleButtonText;

        public static UIWindowExample singleton;

        //AWAKE - Assign Singleton
        protected override void Awake()
        {
            testExampleButtonText.text = "Click to Send!";
            singleton = this;
            base.Awake();
        }
        //SHOW
        public override void Show() { base.Show(); }
        //HIDE
        public override void Hide() { base.Hide(); }
        //THROTTLED UPDATE
        //protected override void ThrottledUpdate() { }

        //ON CLICK TEST BUTTON
        public void OnClickConnect()
        {
            Network.Request.ExampleClientRequestMessage msg = new Network.Request.ExampleClientRequestMessage()
            {
                text = "",
                success = true
            };

            testExampleButtonText.text = "Message Sent!";

            Debug.Log("CLIENT SENDING REQUEST MESSAGE TO SERVER:"
                + "\n" + "SENDING: " + msg.exampleMessage);
            Mirror.NetworkClient.Send<Network.Request.ExampleClientRequestMessage>(msg);
        }
    }
}
