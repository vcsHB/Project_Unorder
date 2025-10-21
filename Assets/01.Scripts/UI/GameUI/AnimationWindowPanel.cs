using UnityEngine;
namespace Project_Unorder.UIManage.InGameSceneUI
{

    public class AnimationWindowPanel : MonoBehaviour, IWindowPanel
    {
        [SerializeField] private Animation _animationCompo;
        [SerializeField] private string _openClipNameKey;
        [SerializeField] private string _closeClipNameKey;
        private CanvasGroup _canvasGroup;

        protected virtual void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();


        }

        [ContextMenu("DebugOpen")]
        public void Open()
        {
            _animationCompo.Play(_openClipNameKey);
        }

        [ContextMenu("DebugClose")]
        public void Close()
        {
            _animationCompo.Play(_closeClipNameKey);

        }
    }
}