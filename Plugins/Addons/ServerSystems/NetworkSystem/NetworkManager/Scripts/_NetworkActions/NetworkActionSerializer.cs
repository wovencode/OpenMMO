using Mirror;
using UnityEngine;

namespace OpenMMO.Network
{
    public static class NetworkActionSerializer
    {
        public static void WriteNetworkAction(this NetworkWriter writer, NetworkAction action)
        {
            writer.WriteInt((int)action);
        }

        public static NetworkAction ReadNetworkAction(this NetworkReader reader)
        {
            //NOTE: Using the Error state here may not be appropriate, it was None originally.
            //TODO: Evaluate these statements
            NetworkAction action = NetworkAction.Error;
            action = (NetworkAction)reader.ReadInt();
            if (action == NetworkAction.Error)
            {
#if DEBUG
                Debug.Log("<color=red>WARNING: </color>" + "Failed to load " + "NetworkAction" + " Check for serialization errors in NetworkActionSerializer...");
#endif
                return NetworkAction.Error; //INVALID ACTION - Make sure we do not execute anything accidentally
            }

            return action;
        }
    }
}