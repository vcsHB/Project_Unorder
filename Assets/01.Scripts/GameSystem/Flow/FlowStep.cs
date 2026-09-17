using System;
using UnityEngine;

namespace Project_Unorder.FlowSystem
{
    public enum FlowActionType
    {
        ExceptionText,
    }

    public class FlowStep : MonoBehaviour
    {
        public event Action OnFlowEndEvent;
        public uint flowLevel => _flowLevel;
        public bool IsRunning => _isRunning;

        [SerializeField] private FlowCue[] _cues;
        [SerializeField] private bool _autoStartOnSceneLoad = true;

        private uint _flowLevel;
        private uint _currentCueIndex;
        private bool _isRunning;
        private bool _hasStarted;

        private FlowCue CurrentCue => _cues[_currentCueIndex];

        private void Awake()
        {
            _cues = GetComponentsInChildren<FlowCue>();
            _currentCueIndex = 0;
        }

        private void Start()
        {
            if (_autoStartOnSceneLoad && !_hasStarted)
                StartFlow();
        }

        public void CancelAutoStart()
        {
            _autoStartOnSceneLoad = false;
        }

        internal void PrepareExternalStart(uint level)
        {
            _flowLevel = level;
            _autoStartOnSceneLoad = false;
        }

        public virtual void StartFlow()
        {
            if (_hasStarted) return;
            _hasStarted = true;

            if (_cues == null || _cues.Length == 0)
            {
                Debug.LogError($"FlowStep: Cues did not Set. Force End this Flow. Level:{_flowLevel}");
                EndFlow();
                return;
            }

            _isRunning = true;
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
            _isRunning = false;
            OnFlowEndEvent?.Invoke();
        }

        internal void SetFlowLevel(uint newLevel)
        {
            _flowLevel = newLevel;
        }
    }
}
