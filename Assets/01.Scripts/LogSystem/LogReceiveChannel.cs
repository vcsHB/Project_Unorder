using System;
using UnityEngine;
namespace Project_Unorder.LogSystem
{

    public static class LogReceiveChannel
    {
        public static event Action<LogData> OnLogBroadcastEvent;

        
        
        public static void Broadcast(LogData log)
        {
            OnLogBroadcastEvent.Invoke(log);
        }
    }
}