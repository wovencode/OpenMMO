//BY FHIZ
using System.Collections.Generic;

namespace OpenMMO.Network
{
    // -----------------------------------------------------------------------------------
    // ServerMessageResponseUserPlayerPreviews
    // @Server -> @Client
    // -----------------------------------------------------------------------------------
    /// <summary>
    /// Public Partial class <c>ServerMessageResponseUserPlayerPreviews</c> inherits <c>ServerMessageResponse</c>.
    /// Sent from Server to Client.
    /// Contains an array of <c>PlayerPreview</c>(s) and the max player number.
    /// Also contains a <c>LoadPlayerPreviews</c> method.
    /// </summary>
    public partial struct ServerResponseUserPlayerPreviews : ServerResponse
    {
        internal NetworkAction _action;
        public NetworkAction action { get { return _action; } set { _action = value; } }

        internal bool _success;
        public bool success { get { return _success; } set { _success = value; } }

        internal string _text;
        public string text { get { return _text; } set { _text = value; } }

        internal bool _causesDisconnect;
        public bool causesDisconnect { get { return _causesDisconnect; } set { _causesDisconnect = value; } }

        public PlayerPreview[] players;
        public int maxPlayers;

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
}