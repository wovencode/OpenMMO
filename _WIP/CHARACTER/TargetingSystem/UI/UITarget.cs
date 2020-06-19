//#define ummorpg //NOTE: Comment this out if you are not using ummorpg

using UnityEngine;
using UnityEngine.UI;

namespace OpenMMO.Targeting
{
    public partial class UITarget : MonoBehaviour
    {
        [Header(" [linked target] ")]
        public Targetable target;

        //[Header("activation key")]
        //public KeyCode hotKey = KeyCode.T;

        [Header("ui links")]
        public GameObject panel;
        public Image targetImage;
        public Text targetLabel;
        public Text targetHealth;
        [Header("label style")]
        public bool lowercaseLabel = false;
        public bool uppercaseLabel = false;

        private void OnValidate()
        {
            if (target != null)
            {
                if (targetLabel) targetLabel.text = target.name; //NAME
                if (targetImage) targetImage.sprite = target.icon; //IMAGE

                ApplyLabelStyle();
            }
        }

        void ApplyLabelStyle()
        {
            if (lowercaseLabel && uppercaseLabel) targetLabel.text.ToUpperInvariant();
            else if (uppercaseLabel) targetLabel.text.ToUpper();
            else if (lowercaseLabel) targetLabel.text.ToLower();
        }

        GameObject player;
        void Update()
        {
            if (!player) player = OpenMMO.PlayerComponent.localPlayer;
            if (player != null)
            {
                TargetingSystem targetingSystem = player.GetComponent<TargetingSystem>();
                if (targetingSystem != null) target = targetingSystem.currentTarget;
                if (target != null) { panel.SetActive(true); } else { panel.SetActive(false); return; }

                // only update the panel if it's active
                if (panel.activeSelf)
                {
                    targetImage.sprite = target.icon; //IMAGE
                    targetLabel.text = target.name; //NAME

                    ApplyLabelStyle();
                }
            }
            else panel.SetActive(false);
        }
    }
}