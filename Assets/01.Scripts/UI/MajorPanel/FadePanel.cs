using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
namespace Project_Unorder.UIManage
{
    [RequireComponent(typeof(CanvasGroup))]
    public class FadePanel : MonoBehaviour, IWindowPanel
    {
        public event Action<FadePanel> OnPanelEnabledEvent;
        public event Action<FadePanel> OnPanelDisabledEvent;
        public UnityEvent OnPanelOpenEvent;
        public UnityEvent OnPanelCloseEvent;
        [SerializeField] protected float _openDuration = 0.4f;
        [SerializeField] protected float _closeDuration = 0.4f;
        [SerializeField] protected bool _useUnscaledTime;

        protected CanvasGroup _canvasGroup;

        [field: SerializeField] public bool IsPanelEnabled { get; protected set; } = true;

        protected virtual void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        #region External Functions

        public virtual void Open()
        {
            if (IsPanelEnabled) return;
            IsPanelEnabled = true;
            OnPanelOpenEvent?.Invoke();
            OnPanelEnabledEvent?.Invoke(this);
            _canvasGroup.DOFade(1f, _openDuration).OnComplete(() =>
            {
                SetInteractable(true);
            }).SetUpdate(_useUnscaledTime);
        }
        public virtual void Close()
        {
            if (!IsPanelEnabled) return;
            SetInteractable(false);
            _canvasGroup.DOFade(0f, _closeDuration).OnComplete(() =>
            {
                IsPanelEnabled = false;
                OnPanelCloseEvent?.Invoke();
                OnPanelDisabledEvent?.Invoke(this);
            }).SetUpdate(_useUnscaledTime);
        }

        #endregion
        protected void SetInteractable(bool value)
        {
            _canvasGroup.interactable = value;
            _canvasGroup.blocksRaycasts = value;
        }

#if UNITY_EDITOR

        protected virtual void OnValidate()
        {
            if (_canvasGroup == null)
                _canvasGroup = GetComponent<CanvasGroup>();
            // Check one more time for SAFE
            if (_canvasGroup == null)
            {
                Debug.LogWarning("CanvasGroup is not Attached");
                return;
            }
            SetInteractable(IsPanelEnabled);
            _canvasGroup.alpha = IsPanelEnabled ? 1f : 0f;
        }
#endif


    }
}