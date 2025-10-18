using UnityEngine;
namespace Project_Unorder.LogSystem
{
    public class LogController : MonoBehaviour
    {
        [SerializeField] private LogGroup _logGroup;

        public void PrintLog(LogData logData)
        {
            LogItem logItem = _logGroup.GetLogItem();
            logItem.SetLogData(logData);
        }
    }
}