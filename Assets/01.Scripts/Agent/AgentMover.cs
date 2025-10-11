using Project_Unorder.PhysicsSystem;
using UnityEngine;
namespace Project_Unorder.AgentSytstem
{

    public class AgentMover : MonoBehaviour, IAgentComponent
    {
        protected PhysicsBody _physicsCompo;
        protected Agent _owner;
        public Vector2 CurrentVelocity => _physicsCompo.Velocity;
        public bool canMove = true;

        protected virtual void Awake()
        {
            _physicsCompo = GetComponent<PhysicsBody>();
        }

        protected virtual void FixedUpdate()
        {
            if (!canMove) return;
            // if (!isEdgeMove)
            // {
            //     Velocity = _moveDirection * _player.PlayerStatus.moveSpeed.GetValue();
            //     _rigidCompo.linearVelocity = Velocity;

            //     OnMovement?.Invoke(Velocity);
            // }
        }



        public virtual void AfterInitialize()
        {
        }

        public virtual void Dispose()
        {
        }

        public virtual void Initialize(Agent owner)
        {
            _owner = owner;
        }

        public virtual void LateInitialize()
        {
        }

        public virtual void SetVelocity(Vector2 velocity)
        {
            _physicsCompo.SetVelocity(velocity);
        }

        public void StopImmediately()
        {
            SetVelocity(Vector2.zero);

        }



    }
}