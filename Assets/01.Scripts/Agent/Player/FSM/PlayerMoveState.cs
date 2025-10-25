using UnityEngine;
namespace Project_Unorder.AgentSystem.PlayerManage.FSM
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
            _mover.SetMovement(direction);


            if (Mathf.Approximately(_player.PlayerInput.InputDirection.magnitude, 0))
            {
                _stateMachine.ChangeState(PlayerStateType.Idle);
            }

        }


        public override void Exit()
        {
            base.Exit();

        }

    }

}