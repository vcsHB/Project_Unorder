using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace Project_Unorder.LogSystem
{

    public class LogItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _logContentText;
        [SerializeField] private Image _logIconImage;

        public void SetLogData(LogData logdata)
        {
            _logIconImage.sprite = logdata.logIcon;
            _logContentText.text = logdata.logContent;
            
            _logContentText.color = logdata.logColor;
            _logIconImage.color = logdata.logColor;
        }
    }
}