//BY FHIZ
//MODIFIED BY DX4D

using System.Collections.Generic;




// S E R V E R  R E S P O N S E  M E S S A G E S - HANDLED ON CLIENT
// @Server -> @Client
namespace OpenMMO.Network.Response
{
    //ERROR
    /// <summary>
    /// Public Partial class <c>ServerResponseError</c> inherits <c>ServerResponse</c>.
    /// Sent from Server to Client.
    /// </summary>
    public partial struct Error : ServerResponse
    {
        internal NetworkAction _action;
        public NetworkAction action { get { return _action; } set { _action = value; } }

        internal bool _success;
        public bool success { get { return _success; } set { _success = value; } }

        internal string _text;
        public string text { get { return _text; } set { _text = value; } }

        internal bool _causesDisconnect;
        public bool causesDisconnect { get { return _causesDisconnect; } set { _causesDisconnect = value; } }
    }
    //CONFIRM
    /// <summary>
    /// Public Partial class <c>ServerMessageResponseUserConfirm</c> inherits <c>ServerMessageResponse</c>.
    /// Sent from Server to Client.
    /// </summary>
    public partial struct UserConfirm : ServerResponse
    {
        internal NetworkAction _action;
        public NetworkAction action { get { return _action; } set { _action = value; } }

        internal bool _success;
        public bool success { get { return _success; } set { _success = value; } }

        internal string _text;
        public string text { get { return _text; } set { _text = value; } }

        internal bool _causesDisconnect;
        public bool causesDisconnect { get { return _causesDisconnect; } set { _causesDisconnect = value; } }
    }
    // PLAYER PREVIEWS
    /// <summary>
    /// Public Partial class <c>ServerMessageResponseUserPlayerPreviews</c> inherits <c>ServerMessageResponse</c>.
    /// Sent from Server to Client.
    /// Contains an array of <c>PlayerPreview</c>(s) and the max player number.
    /// Also contains a <c>LoadPlayerPreviews</c> method.
    /// </summary>
    public partial struct UserPlayerPreviews : ServerPlayerPreviewsResponse
    {
        internal NetworkAction _action;
        public NetworkAction action { get { return _action; } set { _action = value; } }

        internal bool _success;
        public bool success { get { return _success; } set { _success = value; } }

        internal string _text;
        public string text { get { return _text; } set { _text = value; } }

        internal bool _causesDisconnect;
        public bool causesDisconnect { get { return _causesDisconnect; } set { _causesDisconnect = value; } }

        internal PlayerPreview[] _players;
        public PlayerPreview[] players { get { return _players; } set { _players = value; } }

        internal int _maxPlayers;
        public int maxPlayers { get { return _maxPlayers; } set { _maxPlayers = value; } }

        // -------------------------------------------------------------------------------
        // LoadPlayerPreviews
        // -------------------------------------------------------------------------------
        /// <summary>
        /// Public method <c>LoadPlayerPreviews</c> located inside <c>ServerMessageResponseUserPlayerPreviews</c>.
        /// Loads an array of players  previews from a list of players
        /// </summary>
        /// <param name="_players"></param>
        public void LoadPlayerPreviews(List<PlayerPreview> _players)
        {
            players = new PlayerPreview[_players.Count];
            players = _players.ToArray();
        }
    }
    //LOGIN
    /// <summary>
    /// Public Partial class <c>ServerMessageResponseUserLogin</c> inherits <c>ServerMessageResponseUserPlayerPreviews</c>.
    /// Sent from Server to Client.
    /// Based on ServerMessageResponseUserPlayerPreviews. Contains only inherited functionality.
    /// </summary>
    public partial struct UserLogin : ServerLoginUserResponse
    {
        internal NetworkAction _action;
        public NetworkAction action { get { return _action; } set { _action = value; } }

        internal bool _success;
        public bool success { get { return _success; } set { _success = value; } }

        internal string _text;
        public string text { get { return _text; } set { _text = value; } }

        internal bool _causesDisconnect;
        public bool causesDisconnect { get { return _causesDisconnect; } set { _causesDisconnect = value; } }

        internal PlayerPreview[] _players;
        public PlayerPreview[] players { get { return _players; } set { _players = value; } }

        internal int _maxPlayers;
        public int maxPlayers { get { return _maxPlayers; } set { _maxPlayers = value; } }

        // -------------------------------------------------------------------------------
        // LoadPlayerPreviews
        // -------------------------------------------------------------------------------
        /// <summary>
        /// Public method <c>LoadPlayerPreviews</c> located inside <c>ServerMessageResponseUserPlayerPreviews</c>.
        /// Loads an array of players  previews from a list of players
        /// </summary>
        /// <param name="_players"></param>
        public void LoadPlayerPreviews(List<PlayerPreview> _players)
        {
            players = new PlayerPreview[_players.Count];
            players = _players.ToArray();
        }
    }
    //REGISTER
    /// <summary>
    /// Public Partial class <c>ServerMessageResponseUserRegister</c> inherits <c>ServerMessageResponse</c>.
    /// Sent from Server to Client.
    /// </summary>
    public partial struct UserRegister : ServerResponse
    {
        internal NetworkAction _action;
        public NetworkAction action { get { return _action; } set { _action = value; } }

        internal bool _success;
        public bool success { get { return _success; } set { _success = value; } }

        internal string _text;
        public string text { get { return _text; } set { _text = value; } }

        internal bool _causesDisconnect;
        public bool causesDisconnect { get { return _causesDisconnect; } set { _causesDisconnect = value; } }
    }
    //DELETE
    /// <summary>
    /// Public Partial class <c>ServerMessageResponseUserDelete</c> inherits <c>ServerMessageResponse</c>.
    /// Sent from Server to Client.
    /// </summary>
    public partial struct UserDelete : ServerResponse
    {
        internal NetworkAction _action;
        public NetworkAction action { get { return _action; } set { _action = value; } }

        internal bool _success;
        public bool success { get { return _success; } set { _success = value; } }

        internal string _text;
        public string text { get { return _text; } set { _text = value; } }

        internal bool _causesDisconnect;
        public bool causesDisconnect { get { return _causesDisconnect; } set { _causesDisconnect = value; } }
    }
    //CHANGE PASSWORD
    /// <summary>
    /// Public Partial class <c>ServerMessageResponseUserChangePassword</c> inherits <c>ServerMessageResponse</c>.
    /// Sent from Server to Client.
    /// </summary>
    public partial struct UserChangePassword : ServerResponse
    {
        internal NetworkAction _action;
        public NetworkAction action { get { return _action; } set { _action = value; } }

        internal bool _success;
        public bool success { get { return _success; } set { _success = value; } }

        internal string _text;
        public string text { get { return _text; } set { _text = value; } }

        internal bool _causesDisconnect;
        public bool causesDisconnect { get { return _causesDisconnect; } set { _causesDisconnect = value; } }
    }
}
