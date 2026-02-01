using System;
using UnityEngine;
using UnityEngine.Events;
namespace Project_Unorder.FlowSystem
{

    public abstract class FlowCue : MonoBehaviour
    {
        public UnityEvent OnCueCompleteUnityEvent;
        public event Action<float> OnCueCompleteEvent; // _delay
        [SerializeField] protected float _delayToNextCue = 1f;

        public abstract bool Execute();

        protected void InvokeCueComplete()
        {
            OnCueCompleteEvent?.Invoke(_delayToNextCue);
            OnCueCompleteUnityEvent?.Invoke();
        }
    }
}