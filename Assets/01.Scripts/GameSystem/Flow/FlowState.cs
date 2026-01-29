using System;
using Project_Unorder.Core.Attribute;
using UnityEngine;
namespace Project_Unorder.FlowSystem
{

    public abstract class FlowState : MonoBehaviour
    {
        public event Action OnFlowEndEvent;
        [ReadOnly] public uint flowLevel => _flowLevel;
        private uint _flowLevel;
        

        public abstract void StartFlow();


        protected virtual void EndFlow()
        {
            OnFlowEndEvent?.Invoke();
        }




        internal void SetFlowLevel(uint newLevel)
        {
            _flowLevel = newLevel;
        }
    }
}