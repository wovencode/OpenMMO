
using OpenMMO.Network;
using OpenMMO;
using System;

namespace OpenMMO.Network
{

	public enum NetworkState 	{
		Offline,
		Lobby,
		Game
	}
	
	public enum NetworkType
	{
		Client,
		Server,
		HostAndPlay/*,
		Development*/
	}
	
}