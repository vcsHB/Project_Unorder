namespace Project_Unorder.AgentSystem.PlayerManage.FSM
{

    public class PlayerGroundState : OrderPlayerState
    {
        public PlayerGroundState(OrderPlayer player, PlayerStateMachine stateMachine, int animationHash) : base(player, stateMachine, animationHash)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _owner.PlayerInput.OnAttackEvent += _attacker.HandleAttack;
        }

        public override void Exit()
        {
            base.Exit();
            _owner.PlayerInput.OnAttackEvent -= _attacker.HandleAttack;
        }
    }
}