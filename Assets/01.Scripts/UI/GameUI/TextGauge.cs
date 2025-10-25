using TMPro;
using UnityEngine;
namespace Project_Unorder.UIManage.InGameSceneUI
{

    public class TextGauge : MonoBehaviour
    {
        [SerializeField] private TextMeshPro _gaugeText;
        [SerializeField] private uint _maxGaugeLength = 10;

        public void SetGaugeFill(float current, float max)
        {
            if (Mathf.Approximately(max, 0f))
            {
                Debug.LogError("[TextGauge] MaxValue Can't be ZERO. (zero divide exception)");
                return;
            }
            SetGaugeFill(current / max);
        }

        public void SetGaugeFill(float ratio)
        {
            float clampedRatio = Mathf.Clamp01(ratio);
            if (_gaugeText == null)
            {
                Debug.LogError("_gaugeText not Binded");
                return;
            }
            int fillLength = (int)(clampedRatio * _maxGaugeLength);
            _gaugeText.maxVisibleCharacters = fillLength;
        }

    }
}