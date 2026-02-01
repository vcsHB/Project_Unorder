using System;
using UnityEngine;

namespace Project_Unorder.FlowSystem
{
    public enum ActionType
    {
        ExceptionText,
    }

    public class FlowStep : MonoBehaviour
    {
        public event Action OnFlowEndEvent;
        public uint flowLevel => _flowLevel;
        private uint _flowLevel;
        [SerializeField] private FlowCue[] _cues;
        private uint _currentCueIndex = 0;
        private FlowCue CurrentCue => _cues[_currentCueIndex];

        private void Awake()
        {
            _cues = GetComponentsInChildren<FlowCue>();
            _currentCueIndex = 0;
            StartFlow();
        }

        public virtual void StartFlow()
        {
            if (_cues == null || _cues.Length == 0)
            {
                Debug.LogError($"FlowStep: Cues did not Set. Force End this Flow. Level:{_flowLevel}");
                EndFlow();
                return;
            }

            _currentCueIndex = 0;
            ExecuteCurrentCue();
        }

        private void ExecuteCurrentCue()
        {
            CurrentCue.OnCueCompleteEvent += HandleCueComplete;
            Debug.Log($"EXECUTED in flowIndex:{_flowLevel} : FlowCue(idx{_currentCueIndex}) Enter.");
            CurrentCue.Execute();

        }

        private void HandleCueComplete(float delayTime)
        {
            CurrentCue.OnCueCompleteEvent -= HandleCueComplete;
            Debug.Log($"EXECUTED in flowIndex:{_flowLevel} : FlowCue(idx:{_currentCueIndex}) Complete. delay:{delayTime}s");
            if (delayTime > 0f)
                Invoke(nameof(MoveToNextCue), delayTime);
            else
                MoveToNextCue();
        }

        private void MoveToNextCue()
        {
            _currentCueIndex++;

            bool isContinue = _currentCueIndex < _cues.Length;
            if (isContinue)
            {
                ExecuteCurrentCue();
            }
            else
            {
                EndFlow();
            }
        }

        protected virtual void EndFlow()
        {
            OnFlowEndEvent?.Invoke();
        }

        internal void SetFlowLevel(uint newLevel)
        {
            _flowLevel = newLevel;
        }
    }
}