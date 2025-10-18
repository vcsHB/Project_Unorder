using System;
using System.Collections.Generic;
using UnityEngine;
namespace Project_Unorder.LogSystem
{

    public class LogGroup : MonoBehaviour
    {
        [SerializeField] private LogItem _logItemPrefab;
        [SerializeField] private uint _logMaxLength = 20;
        private Stack<LogItem> _itemPool;
        private Queue<LogItem> _logDataQueue;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            _itemPool = new();
            _logDataQueue = new();
            for (int i = 0; i < _logMaxLength; i++)
            {
                LogItem log = CreateLogItem();
                log.gameObject.SetActive(false);
                _itemPool.Push(log);
            }
        }

        public void Clear()
        {
            int amount = _logDataQueue.Count;
            for (int i = 0; i < amount; i++)
            {
                LogItem logItem = _logDataQueue.Dequeue();
                logItem.gameObject.SetActive(false);
                _itemPool.Push(logItem);
            }
        }

        public LogItem GetLogItem()
        {
            LogItem log = _itemPool.Count > 0 ?
            _itemPool.Pop() : _logDataQueue.Dequeue();

            log.gameObject.SetActive(true);
            return log;
        }

        private LogItem CreateLogItem()
        {
            return Instantiate(_logItemPrefab, transform);
        }
    }
}