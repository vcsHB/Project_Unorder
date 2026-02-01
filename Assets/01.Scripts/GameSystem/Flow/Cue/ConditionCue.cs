using UnityEngine;
namespace Project_Unorder.FlowSystem
{

    public abstract class ConditionCue : FlowCue
    {
        
        public override bool Execute()
        {
            return false;
        }
    }
}