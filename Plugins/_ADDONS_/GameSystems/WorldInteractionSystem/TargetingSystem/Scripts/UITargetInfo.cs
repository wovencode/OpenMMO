//BY DX4D

using UnityEngine;
using UnityEngine.UI;

namespace OpenMMO
{
    public partial class UITargetInfo : MonoBehaviour
    {
        [Header(" [linked target] ")]
        public Targetable target;
        
        [Header("ui links")]
        public GameObject panel;
        public Image targetImage;
        public Text targetLabel;

        private void OnValidate()
        {
            if (target != null)
            {
                if (targetLabel) targetLabel.text = target.name; //NAME
                if (targetImage) targetImage.sprite = target.icon; //IMAGE
            }
        }

        GameObject player;
        void Update()
        {
            if (!player) player = OpenMMO.PlayerAccount.localPlayer;
            if (player != null)
            {
                TargetingSystem targetingSystem = player.GetComponent<TargetingSystem>(); //GET TARGETING SYSTEM
                if (targetingSystem != null) target = targetingSystem.currentTarget; //GET CURRENT TARGET
                if (target != null) { panel.SetActive(true); } else { panel.SetActive(false); return; } //ACTIVATE IF THERE IS A TARGET
                
                if (panel.activeSelf)
                {
                    targetImage.sprite = target.icon; //IMAGE
                    targetLabel.text = target.name; //NAME
                }
            }
            else panel.SetActive(false);
        }
    }
}