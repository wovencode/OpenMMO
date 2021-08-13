//BY DX4D

namespace OpenMMO.Network
{
    /// <summary>
    /// Public Enum <c>NetworkState</c> dictates the state which the network is in. The state cna either be offline, lobby, or game
    /// </summary>
	public enum NetworkState
    {
        /// <summary>
        /// The network if offline and not connected
        /// </summary>
		Offline,
        /// <summary>
        /// The network is online and connceted however the user isn't activley playing the game
        /// </summary>
		Lobby,
        /// <summary>
        /// The network is online and connected and the user is activly playing on a player. 
        /// </summary>
		Game
    }
}
