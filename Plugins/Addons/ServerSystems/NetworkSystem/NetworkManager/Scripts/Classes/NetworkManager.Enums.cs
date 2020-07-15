
using OpenMMO.Network;
using OpenMMO;
using System;

namespace OpenMMO.Network
{
    /// <summary>
    /// Public Enum <c>NetworkState</c> dictates the state which the network is in. The state cna either be offline, lobby, or game
    /// </summary>
	public enum NetworkState 	{
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