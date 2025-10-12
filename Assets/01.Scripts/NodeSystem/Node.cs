using UnityEngine;
using UnityEngine.Events;

namespace Project_Unorder.NodeSystem
{

    public abstract class Node : MonoBehaviour
    {
        public UnityEvent OnNodeSelectEvent;
        
        public virtual void Select()
        {
            OnNodeSelectEvent?.Invoke();

        }

        
    }

}