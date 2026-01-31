using System;
using UnityEngine;
namespace Project_Unorder.FlowSystem
{
    
    public abstract class FlowCue : MonoBehaviour
    {
        public event Action OnCueActionCompleteEvent;
        public abstract void Execute();

        protected void InvokeActionComplete()
        {
            OnCueActionCompleteEvent?.Invoke();
        }
    }
}