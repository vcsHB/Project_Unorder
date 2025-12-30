using UnityEngine;
namespace Project_Unorder.AgentSystem.PlayerManage.FSM
{

    public class OrderPlayerState
    {

        protected OrderPlayer _owner;
        protected PlayerStateMachine _stateMachine;
        protected PlayerMover _mover;
        protected OrderPlayerAttacker _attacker;

        protected Animator _animator;
        protected PlayerRenderer _playerRenderer;
        protected int _animationHash;

        public OrderPlayerState(OrderPlayer player, PlayerStateMachine stateMachine, int animationHash)
        {
            _owner = player;
            _stateMachine = stateMachine;
            _mover = player.GetCompo<PlayerMover>(true);
            _attacker = player.GetCompo<OrderPlayerAttacker>();
            _playerRenderer = player.GetCompo<PlayerRenderer>(true);
            _animationHash = animationHash;
        }

        public virtual void Enter()
        {
        }

        public virtual void UpdateState()
        {
        }

        public virtual void Exit()
        {
        }
    }
}