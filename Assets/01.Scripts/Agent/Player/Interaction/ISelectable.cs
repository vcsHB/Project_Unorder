using UnityEngine;
namespace Project_Unorder.AgentSystem.InteractSystem
{
    public interface ISelectable
    {
        public bool Select();

        public void Release();
    }
}