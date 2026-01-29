using UnityEngine;
namespace Project_Unorder.FlowSystem
{

    public class FlowManager : MonoBehaviour
    {
        [SerializeField] private uint _currentFlowLevel;
        [SerializeField] private FlowGroup _flowGroup;


        public void StartFlow(uint level)
        {
            FlowState currentFlowState = Instantiate(GetFlow(level), transform);
            currentFlowState.StartFlow();
        }

        private FlowState GetFlow(uint level)
        {
            if(_flowGroup == null)
            {
                Debug.LogError("FlowGroup is null in FlowManager");
                return null;
            }
            return _flowGroup.GetFlow(level);
        }
           
    }
}