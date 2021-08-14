//by  Fhiz
using OpenMMO;
using OpenMMO.Network;
using OpenMMO.UI;
using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace OpenMMO.UI
{

	/// <summary>
	/// This partial section of UIWindowPlayerCreate updates the available player prefabs.
	/// </summary>
	public partial class UIWindowPlayerCreate
	{
		
		/// <summary>
		/// Hooks into ThrottledUpdate of UIWindowPlayerCreate
		/// </summary>
		[DevExtMethods(nameof(ThrottledUpdate))]
		void ThrottledUpdate_PlayerAccount()
		{
			UpdatePlayerPrefabs();
		}
		
		/// <summary>
		/// Updates the available player prefabs to reflect changes.
		/// </summary>
		protected void UpdatePlayerPrefabs(bool forced=false)
		{

			if (!forced && contentViewport.childCount > 0)
				return;
			
			for (int i = 0; i < contentViewport.childCount; i++)
				GameObject.Destroy(contentViewport.GetChild(i).gameObject);
			
			int _index = 0;
			
			foreach (GameObject player in networkManager.playerPrefabs)
			{

				GameObject go = GameObject.Instantiate(slotPrefab.gameObject);
				go.transform.SetParent(contentViewport.transform, false);

				go.GetComponent<UISelectPlayerSlot>().Init(buttonGroup, _index, player.name, (_index == 0) ? true : false);
				_index++;
			}
			
			index = 0;

		}
		
	}

}