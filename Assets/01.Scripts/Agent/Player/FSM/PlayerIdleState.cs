using UnityEngine;
namespace Project_Unorder.AgentSytstem.PlayerManage.FSM
{

    public class PlayerIdleState : PlayerGroundState
    {
        public PlayerIdleState(Player player, PlayerStateMachine stateMachine, int animationHash) : base(player, stateMachine, animationHash)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _mover.StopImmediately();
        }



    }
}