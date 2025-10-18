using UnityEngine;
namespace Project_Unorder.LogSystem
{
    public class LogController : MonoBehaviour
    {
        [SerializeField] private LogGroup _logGroup;
        [SerializeField] private bool _logChannelEnable;

#if UNITY_EDITOR
        [Header("Log Debugger")]
        [SerializeField]
        private LogData _testLogContent;
#endif

        private void Awake()
        {
            if (_logChannelEnable)
                LogReceiveChannel.OnLogBroadcastEvent += PrintLog;
        }

        private void OnDestroy()
        {
            if (_logChannelEnable)
                LogReceiveChannel.OnLogBroadcastEvent -= PrintLog;

        }

        public void SetLogChannelEnable(bool value)
        {
            if (_logChannelEnable && !value)
            {
                // Set Disable
                LogReceiveChannel.OnLogBroadcastEvent -= PrintLog;
            }
            else if (!_logChannelEnable && value)
            {
                // Set Enable
                LogReceiveChannel.OnLogBroadcastEvent += PrintLog;
            }
            _logChannelEnable = value;
        }

        public void PrintLog(LogData logData)
        {
            if (_logGroup == null)
            {
                Debug.LogError("[LogController] _logGroup is not Binded");
                return;
            }
            LogItem logItem = _logGroup.GetLogItem();
            logItem.SetLogData(logData);
        }


#if UNITY_EDITOR
        [ContextMenu("DebugPrintLog")]
        private void DebugPrintLog()
        {
            PrintLog(_testLogContent);
        }
#endif

    }
}