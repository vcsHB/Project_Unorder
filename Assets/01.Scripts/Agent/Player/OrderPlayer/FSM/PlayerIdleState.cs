using UnityEngine;
namespace Project_Unorder.AgentSystem.PlayerManage.FSM
{

    public class PlayerIdleState : PlayerGroundState
    {
        public PlayerIdleState(OrderPlayer player, PlayerStateMachine stateMachine, int animationHash) : base(player, stateMachine, animationHash)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _mover.StopImmediately();

        }

        public override void UpdateState()
        {
            base.UpdateState();
            float magnitude = _owner.PlayerInput.InputDirection.magnitude;
            if (Mathf.Abs(magnitude) > 0f)
            {
                _stateMachine.ChangeState(PlayerStateType.Move);
            }
        }

    }
}