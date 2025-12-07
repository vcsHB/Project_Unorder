using UnityEngine;
using UnityEngine.UI;

namespace Project_Unorder.UIManage
{
    public delegate void OnValueChangeEvent(float currentValue, float maxValue);
    public class UIGauge : MonoBehaviour
    {
        private enum GaugeState
        {
            Idle,
            FillingUp,
            ReducingMain,
            ReducingSub
        }

        [SerializeField] private Image _gaugeMainFillImage;
        [SerializeField] private Image _gaugeSubFillImage;
        [SerializeField] private float _mainGaugeFillDuration = 0.3f;
        [SerializeField] private float _subGaugeFillDuration = 1f;
        [SerializeField] private float _subDelay = 0.3f;

        private float _targetValue;
        private float _mainVelocity;
        private float _subVelocity;
        private float _subDelayTimer;


        private GaugeState _state = GaugeState.Idle;

        public void SetGaugeFillImmediately(float amount)
        {
            amount = Mathf.Clamp01(amount);
            if (Mathf.Approximately(amount, _targetValue))
                return;
            _targetValue = amount;
            _gaugeMainFillImage.fillAmount = _targetValue;
        }

        public void SetGaugeFill(float amount)
        {
            amount = Mathf.Clamp01(amount);

            if (Mathf.Approximately(amount, _targetValue))
                return;

            _targetValue = amount;

            float current = _gaugeMainFillImage.fillAmount;

            if (amount > current)
            {
                _state = GaugeState.FillingUp;
                _subDelayTimer = 0;
            }
            else
            {
                _state = GaugeState.ReducingMain;
                _subDelayTimer = _subDelay;
            }
        }

        private void Update()
        {
            switch (_state)
            {
                case GaugeState.FillingUp:
                    UpdateFillUp();
                    break;

                case GaugeState.ReducingMain:
                    UpdateReduceMain();
                    break;

                case GaugeState.ReducingSub:
                    UpdateReduceSub();
                    break;
            }
        }

        #region Gauge FSM

        private void UpdateFillUp()
        {
            float current = _gaugeMainFillImage.fillAmount;
            current = Mathf.MoveTowards(current, _targetValue, Time.deltaTime / _mainGaugeFillDuration);
            _gaugeMainFillImage.fillAmount = current;

            _gaugeSubFillImage.fillAmount = current; // Sub 즉시 따라감

            if (Mathf.Approximately(current, _targetValue))
                _state = GaugeState.Idle;
        }

        private void UpdateReduceMain()
        {
            float current = _gaugeMainFillImage.fillAmount;
            current = Mathf.MoveTowards(current, _targetValue, Time.deltaTime / _mainGaugeFillDuration);
            _gaugeMainFillImage.fillAmount = current;

            if (_subDelayTimer > 0)
            {
                _subDelayTimer -= Time.deltaTime;
                return;
            }

            if (Mathf.Approximately(current, _targetValue))
                _state = GaugeState.ReducingSub;
        }

        private void UpdateReduceSub()
        {
            float currentSub = _gaugeSubFillImage.fillAmount;
            currentSub = Mathf.MoveTowards(currentSub, _targetValue, Time.deltaTime / _subGaugeFillDuration);
            _gaugeSubFillImage.fillAmount = currentSub;

            if (Mathf.Approximately(currentSub, _targetValue))
                _state = GaugeState.Idle;
        }
        #endregion
    }
}
