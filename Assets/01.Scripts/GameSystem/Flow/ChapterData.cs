using UnityEngine;
namespace Project_Unorder.FlowSystem
{
    [CreateAssetMenu(menuName = "SO/Flow/Chapter")]
    public class ChapterData : ScriptableObject
    {
        public FlowStep[] flowStates;

        public FlowStep GetFlow(uint step)
        {
            if (flowStates == null)
            {
                Debug.LogError("FlowGroup:states is null");
                return null;
            }
            if (flowStates.Length <= step)
            {
                Debug.LogError("FlowLevel to find is out of Range");
                return null;
            }
            return flowStates[step];
        }
    }
}