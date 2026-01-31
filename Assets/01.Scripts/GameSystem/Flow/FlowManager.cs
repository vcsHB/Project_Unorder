using UnityEngine;
namespace Project_Unorder.FlowSystem
{

    public class FlowManager : MonoBehaviour
    {
        [Header("Essential Data")]
        [SerializeField] private ChapterGroupData _chapterGroupData;
        [Header("Current Values")]
        [SerializeField] private uint _currentFlowLevel;
        [SerializeField] private ChapterData _currentChapter;
        private FlowData _data; // Sync to snapshot


        public void SetCurrentChapter(uint chapterId)
        {
            // TODO : Check Range
            _currentChapter = _chapterGroupData.chapters[chapterId];

        }        

        public void StartFlow(uint level)
        {
            FlowStep currentFlowState = Instantiate(GetFlow(level), transform);
            currentFlowState.StartFlow();
        }

        private FlowStep GetFlow(uint level)
        {
            if (_currentChapter == null)
            {
                Debug.LogError("FlowGroup is null in FlowManager");
                return null;
            }
            return _currentChapter.GetFlow(level);
        }

    }
}