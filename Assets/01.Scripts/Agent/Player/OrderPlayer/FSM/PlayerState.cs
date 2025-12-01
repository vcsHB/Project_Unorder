using UnityEngine;
namespace Project_Unorder.AgentSystem.PlayerManage.FSM
{

    public class PlayerState
    {

        protected OrderPlayer _player;
        protected PlayerStateMachine _stateMachine;
        protected PlayerMover _mover;
        
        protected Animator _animator;
        protected PlayerRenderer _playerRenderer;
        protected int _animationHash;


        public PlayerState(OrderPlayer player, PlayerStateMachine stateMachine, int animationHash)
        {
            _player = player;
            _stateMachine = stateMachine;
            _mover = player.GetCompo<PlayerMover>(true);
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