using DG.Tweening;
using UnityEngine;
namespace Project_Unorder.UIManage.Decoration
{

    public class HoverWidthHighLighter : HoverPanel
    {
        [SerializeField] protected float _defaultWidth = 100f;
        [SerializeField] protected float _enableWidth = 300f;
        [SerializeField] protected float _duration = 0.2f;
        [SerializeField] protected bool _useUnscaledTime;
        private RectTransform _rectTrm;
        private float _panelYSize;
        [SerializeField] private bool _interactable = true;
        protected virtual void Awake()
        {
            _rectTrm = transform as RectTransform;
            _panelYSize = _rectTrm.sizeDelta.y;
            OnMouseEnterEvent.AddListener(HandleMouseEnter);
            OnMouseExitEvent.AddListener(HandleMouseExit);
        }

        public void SetInteractable(bool value)
        {
            _interactable = value;
        }

        private void HandleMouseEnter()
        {
            if (_interactable)
                _rectTrm.DOSizeDelta(new Vector2(_enableWidth, _panelYSize), _duration).SetUpdate(_useUnscaledTime);
        }

        private void HandleMouseExit()
        {
            if (_interactable)
                _rectTrm.DOSizeDelta(new Vector2(_defaultWidth, _panelYSize), _duration).SetUpdate(_useUnscaledTime);
        }
    }
}