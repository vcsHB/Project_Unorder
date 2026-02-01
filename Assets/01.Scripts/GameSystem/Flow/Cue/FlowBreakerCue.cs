using UnityEngine;
namespace Project_Unorder.FlowSystem
{
    public class FlowBreakerCue : FlowCue
    { // Flow PAUSE
        public override void Execute()
        {
        }

        public void EscapeFreeze()
        {
            InvokeCueComplete();
        }
    }
}