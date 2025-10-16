using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Project_Unorder.UIManage.InGameSceneUI
{
    public class DataGauge : MonoBehaviour
    {
        [SerializeField] private Image _mainFillIamge;
        [SerializeField] private Image _subFillImage;
        [SerializeField] private float _fillDuration = 0.2f;

        private Sequence _tweenSequence;

        private void KillCurrentTween()
        {
            _tweenSequence?.Kill();
            _tweenSequence = null;
        }

        public void HandleDataIncrease(float ratio)
        {
            KillCurrentTween(); 

            _tweenSequence = DOTween.Sequence();

            _tweenSequence.Append(_mainFillIamge.DOFillAmount(ratio, _fillDuration));
            _tweenSequence.Append(_subFillImage.DOFillAmount(ratio, _fillDuration));
            _tweenSequence.OnComplete(() => _tweenSequence = null);

            _tweenSequence.Play();
        }

        public void HandleDataDecrease(float ratio)
        {
            KillCurrentTween(); 

            _tweenSequence = DOTween.Sequence();

            _tweenSequence.Append(_subFillImage.DOFillAmount(ratio, _fillDuration));
            _tweenSequence.Append(_mainFillIamge.DOFillAmount(ratio, _fillDuration));
            _tweenSequence.OnComplete(() => _tweenSequence = null);

            _tweenSequence.Play();
        }
    }
}