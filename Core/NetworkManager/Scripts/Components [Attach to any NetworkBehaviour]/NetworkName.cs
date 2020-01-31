// =======================================================================================
// Wovencore
// by Weaver (Fhiz)
// MIT licensed
//
// Attach this script to any GameObject with a NetworkIdentity on it (like your player
// prefab) to remove the (Clone) from its name when it is instantiated.
//
// =======================================================================================

using Wovencode;
using Wovencode.Network;
using Mirror;

namespace Wovencode.Network
{

	public class NetworkName : NetworkBehaviour
	{
   
		public override bool OnSerialize(NetworkWriter writer, bool initialState)
		{
			writer.WriteString(name);
			return true;
		}

		public override void OnDeserialize(NetworkReader reader, bool initialState)
		{
			name = reader.ReadString();
		}
	
	}

}

// =======================================================================================