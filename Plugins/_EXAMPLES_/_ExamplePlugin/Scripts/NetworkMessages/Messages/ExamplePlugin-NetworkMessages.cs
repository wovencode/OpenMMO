//BY DX4D

#region N E T W O R K  M E S S A G E S
// ------------------------------

#region SERVER RESPONSE MESSAGES - HANDLED ON CLIENT
// @Server -> @Client
namespace OpenMMO.Network.Response
{
    /// <summary><para>
    /// </para></summary>
    public partial struct ExampleServerResponseMessage : ServerExampleResponse
    {
        //EXAMPLE: These are the basic fields from ServerResponse
        ///<summary>  NETWORK ACTION
        ///             We set the NetworkAction to None because we are not sending a 
        ///             System Message that needs Validation Checks.
        ///             <para>
        ///             Because the NetworkAction is hard-coded for each message 
        ///             potential cheaters cannot spoof a different message to act like 
        ///             a Server Message by using the same name as that Server Message.
        ///             </para></summary>
        public NetworkAction action => NetworkAction.None;
        /// <summary> SUCCESS
        ///             The success field is used to communicate success/failure 
        ///             between Client and Server</summary>
        internal bool _success;
        public bool success { get { return _success; } set { _success = value; } }
        /// <summary> TEXT
        ///             The text field is used to communicate a string 
        ///             between Client and Server. Normally we would just use 
        ///             this field to accomplish our Example with a basic message.</summary>
        internal string _text;
        public string text { get { return _text; } set { _text = value; } }
        /// <summary> CAUSE DISCONNECT
        ///             When the client receives a message with this bool set to true, 
        ///             that client will be told to disconnect from the Server</summary>
        internal bool _causesDisconnect;
        public bool causesDisconnect { get { return _causesDisconnect; } set { _causesDisconnect = value; } }

        //EXAMPLE: The following field is from ServerExampleResponse
        public string exampleMessage => "Hello Client World! -From the Server";
    }
}
#endregion //SERVER RESPONSE

#region CLIENT REQUEST MESSAGES - HANDLED ON SERVER
// @Client -> @Server
namespace OpenMMO.Network.Request
{
    /// <summary><para>
    /// </para></summary>
    public partial struct ExampleClientRequestMessage : ClientExampleRequest
    {
        //EXAMPLE: These are the basic fields from ClientRequest
        ///<summary>  NETWORK ACTION
        ///             We set the NetworkAction to None because we are not sending a 
        ///             System Message that needs Validation Checks.
        ///             <para>
        ///             Because the NetworkAction is hard-coded for each message 
        ///             potential cheaters cannot spoof a different message to act like 
        ///             a Server Message by using the same name as that Server Message.
        ///             </para></summary>
        public NetworkAction action => NetworkAction.None;
        /// <summary> SUCCESS
        ///             The success field is used to communicate success/failure 
        ///             between Client and Server</summary>
        internal bool _success;
        public bool success { get { return _success; } set { _success = value; } }
        /// <summary> TEXT
        ///             The text field is used to communicate a string 
        ///             between Client and Server. Normally we would just use 
        ///             this field to accomplish our Example with a basic message.</summary>
        internal string _text;
        public string text { get { return _text; } set { _text = value; } }

        //EXAMPLE: The following field is from ClientExampleRequest
        public string exampleMessage => "Hello Server World! -From the Client";
    }
}
#endregion //CLIENT REQUESTS

#endregion //NETWORK MESSAGES
