//BY DX4D

namespace OpenMMO.Network
{
    /// <summary>
    /// Public Enum <c>NetworkType</c> dictates what topology the game should be run in. It can either be client, server or hostandplay
    /// </summary>
	public enum NetworkType
    {
        /// <summary>
        /// Client network type means that the game will only act as client and only run client code
        /// </summary>
		Client,
        /// <summary>
        /// Server network type means that the game will only act as a server and only run server code
        /// </summary>
		Server,
        /// <summary>
        /// The game will setup both client and server networks having a player run the game server
        /// </summary>
		HostAndPlay
    }
}
