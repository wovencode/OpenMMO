//BY DX4D
using UnityEngine;
using UnityEngine.UI;

namespace OpenMMO.UI
{
    /// <summary>Close Application Button</summary>
    [RequireComponent(typeof(Button))]
    public partial class UICloseApplicationButton : MonoBehaviour
    {
#pragma warning disable CS0649
        [Header("BUTTONS")]
        [SerializeField] Button quitButton;
#pragma warning restore CS0649

        //ENABLE
        void OnEnable()
        {
            quitButton.onClick.SetListener(() => { OnCloseButtonClicked(); });
        }
        void OnCloseButtonClicked()
        {
            Quit();
        }
        void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
