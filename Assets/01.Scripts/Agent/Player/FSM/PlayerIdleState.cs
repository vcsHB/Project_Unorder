using UnityEngine;
namespace Project_Unorder.AgentSytstem.PlayerManage.FSM
{

    public class PlayerIdleState : PlayerState
    {
        public PlayerIdleState(Player player, PlayerStateMachine stateMachine, int animationHash) : base(player, stateMachine, animationHash)
        {
        }
    }
}