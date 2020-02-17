
using OpenMMO;
using OpenMMO.Network;
using Mirror;

namespace OpenMMO.Network
{

	// -----------------------------------------------------------------------------------
    /// <summary>
    /// Public class <c>NetworkName</c> inherits from Mirror.NetworkBehaviour.
    /// Attach this script to any GameObject with a NetworkIdentity on it (like your player prefab) to remove the (Clone) from its name when it is instantiated.
    /// </summary>
	public partial class NetworkName : BaseNetworkBehaviour
	{
   		
   		// -------------------------------------------------------------------------------
        /// <summary>
        /// public override function <c>OnSerialize</c> that writes the name to the network stream
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="initialState"></param>
        /// <returns></returns>
		public override bool OnSerialize(NetworkWriter writer, bool initialState)
		{
			writer.WriteString(name);
			return true;
		}
		
		// -------------------------------------------------------------------------------
        /// <summary>
        /// Public override method <c>OnDeserialize</c> that reads the name from the network stream.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="initialState"></param>
		public override void OnDeserialize(NetworkReader reader, bool initialState)
		{
			name = reader.ReadString();
		}
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================