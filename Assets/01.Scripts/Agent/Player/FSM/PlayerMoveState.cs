using UnityEngine;
namespace Project_Unorder.AgentSytstem.PlayerManage.FSM
{

    public class PlayerMoveState : PlayerGroundState
    {
        public PlayerMoveState(Player player, PlayerStateMachine stateMachine, int animationHash) : base(player, stateMachine, animationHash)
        {
        }


        public override void Enter()
        {
            base.Enter();

        }

        public override void UpdateState()
        {
            base.UpdateState();
            Vector2 direction = _player.PlayerInput.InputDirection;
            //Debug.Log("리미트 모드 밍밍 Direction: " + direction);
            _mover.SetVelocity(direction);

        }

        public override void Exit()
        {
            base.Exit();

        }

    }

}