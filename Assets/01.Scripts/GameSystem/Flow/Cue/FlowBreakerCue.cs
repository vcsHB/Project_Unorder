using UnityEngine;
namespace Project_Unorder.FlowSystem
{
    public class FlowBreakerCue : FlowCue
    { // Flow PAUSE
        public override bool Execute()
        {
            return false;
        }

        public void EscapeFreeze()
        {
            InvokeCueComplete();
        }
    }
}