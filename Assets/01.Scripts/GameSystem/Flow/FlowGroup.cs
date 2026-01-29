using UnityEngine;
namespace Project_Unorder.FlowSystem
{
    [CreateAssetMenu(menuName ="SO/FlowGroup")]
    public class FlowGroup : ScriptableObject
    {
        public FlowState[] flowStates;

        public FlowState GetFlow(uint level)
        {
            if(flowStates == null)
            {
                Debug.LogError("FlowGroup:states is null");
                return null;
            }
            if(flowStates.Length <= level)
            {
                Debug.LogError("FlowLevel to find is out of Range");
                return null;
            }
            return flowStates[level];
        }
    }
}