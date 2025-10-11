using Project_Unorder.PhysicsSystem;
using UnityEngine;
namespace Project_Unorder.AgentSystem
{

    public class AgentMover : MonoBehaviour, IAgentComponent
    {
        protected PhysicsBody _physicsCompo;
        protected Agent _owner;
        public Vector2 CurrentVelocity => _physicsCompo.Velocity;
        [SerializeField] private float _moveSpeed = 2f;
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
            _physicsCompo.SetMovement(velocity);
        }
        public virtual void SetMovement(Vector2 direction)
        {
            if (_physicsCompo.IsGravityEnable)
            {

                Vector2 gravityDir = _physicsCompo.GravityDirection.normalized;
                Vector2 perpendicular = direction - Vector2.Dot(direction, gravityDir) * gravityDir;
                _physicsCompo.SetMovement(perpendicular.normalized * _moveSpeed);
            }
            else
            {
                SetVelocity(direction * _moveSpeed);
            }
        }

        public void StopImmediately()
        {
            _physicsCompo.StopMovement();
        }



    }
}