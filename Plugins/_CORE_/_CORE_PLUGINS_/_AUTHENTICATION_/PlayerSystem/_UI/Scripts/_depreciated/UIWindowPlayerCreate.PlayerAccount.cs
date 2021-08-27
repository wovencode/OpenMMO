//by  Fhiz
/* //DEPRECIATED
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
        protected void UpdatePlayerPrefabs(bool forced = false)
        {

            if (!forced && contentHolder.childCount > 0)
                return;

            for (int i = 0; i < contentHolder.childCount; i++)
                GameObject.Destroy(contentHolder.GetChild(i).gameObject);

            int _index = 0;

            foreach (GameObject player in networkManager.playerPrefabs)
            {
                GameObject go = GameObject.Instantiate(slotPrefab.gameObject);
                go.transform.SetParent(contentHolder.transform, false);

                //TODO: The second player.name should be player.prefabname - the current solution works for now
                go.GetComponent<UISelectPlayerSlot>().Init(buttonGroup, _index, player.name, player.name, (_index == 0) ? true : false);
                _index++;
            }

            UpdatePlayerIndex(); //ADDED DX4D
            //index = 0; //REMOVED DX4D
        }
    }
}
*/