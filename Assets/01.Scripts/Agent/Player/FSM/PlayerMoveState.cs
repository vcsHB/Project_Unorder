using UnityEngine;
namespace Project_Unorder.AgentSytstem.PlayerManage.FSM
{

    public class PlayerMoveState : PlayerState
    {
        public PlayerMoveState(Player player, PlayerStateMachine stateMachine, int animationHash) : base(player, stateMachine, animationHash)
        {
        }
    }
}