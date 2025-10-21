using UnityEngine;
namespace Project_Unorder.UIManage.InGameSceneUI.MainUIs
{

    public class DenyPanel : MonoBehaviour, IWindowPanel
    {
        [SerializeField] private Animation _animationCompo;
        private CanvasGroup _canvasGroup;
        private const string _openAnimationName = "DenyPanelOpenAnimation";
        private const string _closeAnimationName = "DenyPanelCloseAnimation";


        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();


        }
        [ContextMenu("DebugOpen")]
        public void Open()
        {
            _animationCompo.Play(_openAnimationName);
        }

        [ContextMenu("DebugClose")]
        public void Close()
        {
            _animationCompo.Play(_closeAnimationName);

        }

    }
}