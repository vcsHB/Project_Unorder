using UnityEngine;
namespace Project_Unorder.PhysicsSystem
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PhysicsBody : MonoBehaviour, IGravityChangeable
    {
        [Header("Gravity Settings")]
        [SerializeField] private Vector2 _gravityDirection = new Vector2(0f, -1f);
        public Vector2 GravityDirection => _gravityDirection;
        public Vector2 GroundNormal => -_gravityDirection;
        [SerializeField] private float _gravityScale = 1f;
        [SerializeField] private float _gravityStrength = 9.8f;
        [SerializeField] private float _groundCheckDistance = 0.1f;
        [SerializeField] private LayerMask _groundLayer = 6;

        private Rigidbody2D _rigidCompo;
        public Vector2 Velocity => _rigidCompo.linearVelocity;
        private Vector2 _externalVelocity;
        private Vector2 _lastVelocity;
        private bool _isGrounded;

        private void Awake()
        {
            _rigidCompo = GetComponent<Rigidbody2D>();
            _rigidCompo.gravityScale = 0f;
        }

        private void FixedUpdate()
        {
            CheckGround();

            if (!_isGrounded)
                ApplyCustomGravity();

            _rigidCompo.linearVelocity = _lastVelocity + _externalVelocity;
            _lastVelocity = _rigidCompo.linearVelocity;
            _externalVelocity = Vector2.zero;
        }

        private void ApplyCustomGravity()
        {
            _lastVelocity += _gravityDirection.normalized * _gravityStrength * _gravityScale * Time.fixedDeltaTime;
        }

        public void SetGravityDirection(Vector2 newDir)
        {
            _gravityDirection = newDir.normalized;
        }
        public void AddForce(Vector2 power)
        {
            _rigidCompo.AddForce(power, ForceMode2D.Impulse);
        }

        public void SetVelocity(Vector2 velocity)
        {
            _externalVelocity = velocity;
        }

        private void CheckGround()
        {
            Vector2 origin = _rigidCompo.position;
            Vector2 dir = _gravityDirection.normalized;
            float distance = _groundCheckDistance;

            RaycastHit2D hit = Physics2D.Raycast(origin, dir, distance, _groundLayer);
            if (hit.collider != null)
            {
                _isGrounded = true;
                _lastVelocity = Vector2.zero;
            }
            else
            {
                _isGrounded = false;
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _groundCheckDistance);
        }

#endif
    }
}
