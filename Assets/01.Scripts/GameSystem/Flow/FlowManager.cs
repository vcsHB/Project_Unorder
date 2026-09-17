using System;
using UnityEngine;

namespace Project_Unorder.FlowSystem
{
    public class FlowManager : MonoBehaviour
    {
        public event Action<uint> OnFlowStartEvent;
        public event Action<uint> OnFlowEndEvent;
        public event Action OnChapterEndEvent;

        [Header("Essential Data")]
        [SerializeField] private ChapterGroupData _chapterGroupData;
        [Header("Current Values")]
        [SerializeField] private uint _currentChapterId;
        [SerializeField] private uint _currentFlowLevel;
        [SerializeField] private ChapterData _currentChapter;
        [Header("Progress Settings")]
        [SerializeField] private bool _advanceToNextStep = true;

        private FlowStep _currentStep;
        private FlowData _data;

        public uint CurrentChapterId => _currentChapterId;
        public uint CurrentFlowLevel => _currentFlowLevel;
        public bool IsFlowRunning => _currentStep != null;
        public FlowData CurrentData => _data;

        public bool SetCurrentChapter(uint chapterId)
        {
            if (_chapterGroupData == null)
            {
                Debug.LogError("FlowManager: ChapterGroupData is not binded");
                return false;
            }

            ChapterData[] chapters = _chapterGroupData.chapters;
            if (chapters == null || chapterId >= chapters.Length)
            {
                Debug.LogError($"FlowManager: ChapterId {chapterId} is out of range (count:{chapters?.Length ?? 0})");
                return false;
            }

            if (chapters[chapterId] == null)
            {
                Debug.LogError($"FlowManager: Chapter {chapterId} is null");
                return false;
            }

            _currentChapterId = chapterId;
            _currentChapter = chapters[chapterId];
            return true;
        }

        public bool StartChapter(uint chapterId, uint step = 0)
        {
            if (!SetCurrentChapter(chapterId)) return false;
            return StartFlow(step);
        }

        public bool StartFlow(uint level)
        {
            FlowStep prefab = GetFlow(level);
            if (prefab == null) return false;

            StopFlow();

            _currentFlowLevel = level;
            _currentStep = Instantiate(prefab, transform);
            _currentStep.PrepareExternalStart(level);
            _currentStep.OnFlowEndEvent += HandleFlowEnd;

            OnFlowStartEvent?.Invoke(level);
            _currentStep.StartFlow();
            return true;
        }

        public void StopFlow()
        {
            if (_currentStep == null) return;

            FlowStep running = _currentStep;
            _currentStep = null;
            running.OnFlowEndEvent -= HandleFlowEnd;
            Destroy(running.gameObject);
        }

        public FlowSnapshot CaptureSnapshot(uint mapId, uint layerIndex, Vector2 position)
        {
            return new FlowSnapshot(_currentChapterId, _currentFlowLevel, mapId, layerIndex, position);
        }

        public bool RestoreSnapshot(FlowData snapshot)
        {
            if (snapshot == null)
            {
                Debug.LogError("FlowManager: Snapshot is null");
                return false;
            }

            _data = snapshot;
            return StartChapter(snapshot.flowChapter, snapshot.step);
        }

        private void HandleFlowEnd()
        {
            uint endedLevel = _currentFlowLevel;
            StopFlow();
            OnFlowEndEvent?.Invoke(endedLevel);

            uint nextLevel = endedLevel + 1;
            if (!_advanceToNextStep || !HasFlow(nextLevel))
            {
                OnChapterEndEvent?.Invoke();
                return;
            }

            StartFlow(nextLevel);
        }

        private bool HasFlow(uint level)
        {
            return _currentChapter != null
                && _currentChapter.flowStates != null
                && level < _currentChapter.flowStates.Length;
        }

        private FlowStep GetFlow(uint level)
        {
            if (_currentChapter == null)
            {
                Debug.LogError("FlowManager: CurrentChapter is null. Call SetCurrentChapter first");
                return null;
            }

            if (!HasFlow(level))
            {
                Debug.LogError($"FlowManager: FlowLevel {level} is out of range in chapter {_currentChapterId}");
                return null;
            }

            return _currentChapter.GetFlow(level);
        }
    }
}
