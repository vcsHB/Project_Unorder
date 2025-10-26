using UnityEngine;

namespace Project_Unorder.ActionSystem
{

    public enum ActionType
    {
        Destory,
        Unorder,
        Attack,
        Fix,
        Mercy,
        Forgive,
        SelfDestroy,

    }
    public class ActionManager : MonoBehaviour
    {

        public void Act(ActionType actionType)
        {
            // TODO: Implement ActionSystm
        }
    }
}