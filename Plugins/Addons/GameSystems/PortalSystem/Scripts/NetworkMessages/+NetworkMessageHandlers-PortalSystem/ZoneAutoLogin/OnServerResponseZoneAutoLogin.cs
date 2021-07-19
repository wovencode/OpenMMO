//BY FHIZ
//MODIFIED BY DX4D
//Execution: @Client
using UnityEngine;
using OpenMMO.Network;
using OpenMMO.UI;

namespace OpenMMO.Zones
{
	public partial class ZoneManager : MonoBehaviour
	{
        //public void OnServerMessageResponsePlayerAutoLogin(NetworkConnection conn, ServerResponsePlayerAutoLogin msg) //REMOVED - DX4D
        public void OnServerResponseZoneAutoLogin(ServerResponseZoneAutoLogin msg) //ADDED - DX4D
        {
        	
        	autoPlayerName = "";
        	
        	if (UIPopupNotify.singleton)
				UIPopupNotify.singleton.Hide();
        }
	}
}