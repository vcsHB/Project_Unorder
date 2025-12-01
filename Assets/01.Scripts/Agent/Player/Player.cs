using InputManage;
using UnityEngine;
namespace Project_Unorder.AgentSystem.PlayerManage
{
    public class Player : Agent
    {
        [field: SerializeField] public PlayerInput PlayerInput { get; private set; }

    }
}