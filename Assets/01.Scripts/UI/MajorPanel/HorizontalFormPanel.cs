using DG.Tweening;
using UnityEngine;
namespace Project_Unorder.UIManage
{

    public class HorizontalFormPanel : SizingPanel
    {
        [Header("Sizing Setting")]
        [SerializeField] private float _enableHeight = 100f;
        [SerializeField] private float _disableHeight = 0f;
        [SerializeField] private float _fadeDuration = 0.1f;
        private float _defaultXSizeDelta;
        protected override void Awake()
        {
            base.Awake();
            _defaultXSizeDelta = _rectTrm.sizeDelta.x;
        }

        public override void Open()
        {
            IsPanelEnabled = true;
            SetInteractable(true);
            OnPanelOpenEvent?.Invoke();
            _canvasGroup.DOFade(1f, _fadeDuration).SetUpdate(_useUnscaledTime);
            _rectTrm.DOSizeDelta(new Vector2(_defaultXSizeDelta, _enableHeight), _openDuration).SetUpdate(_useUnscaledTime);

        }

        public override void Close()
        {
            SetInteractable(false);
            _rectTrm.DOSizeDelta(new Vector2(_defaultXSizeDelta, _disableHeight), _closeDuration).SetUpdate(_useUnscaledTime).OnComplete(() =>
            {
                IsPanelEnabled = false;
                
                OnPanelCloseEvent?.Invoke();
                _canvasGroup.DOFade(0f, _fadeDuration).SetUpdate(_useUnscaledTime);

            });

        }

#if UNITY_EDITOR

        protected override void OnValidate()
        {
            if (_rectTrm == null)
            {
                _rectTrm = transform as RectTransform;
                _defaultXSizeDelta = _rectTrm.sizeDelta.x;
                if (IsPanelEnabled)
                    _enableHeight = _rectTrm.sizeDelta.y;
            }
            if (_rectTrm == null)
            {
                Debug.LogWarning("RectTransform is not Attached [is it possible??]");
                return;
            }
            base.OnValidate();
            _rectTrm.sizeDelta = new Vector2(_defaultXSizeDelta, IsPanelEnabled ? _enableHeight : _disableHeight);
        }
#endif
    }
}