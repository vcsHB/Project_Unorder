using UnityEngine;
namespace Project_Unorder.ObjectManage.CombatObjects.ProcessWindows
{

    public abstract class Process : MonoBehaviour
    {
        public abstract void InitializeProcess();
        public abstract void DestroyProcess();

        public abstract void SetWarningProcess();
        

    }
}