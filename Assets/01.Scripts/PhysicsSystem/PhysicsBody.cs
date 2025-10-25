using UnityEngine;

namespace Project_Unorder.PhysicsSystem
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PhysicsBody : MonoBehaviour, IGravityChangeable
    {
        [Header("Gravity Settings")]
        [SerializeField] private Vector2 _gravityDirection = new Vector2(0f, -1f);
        public Vector2 GravityDirection => _gravityDirection;
        public Vector2 GroundNormal => -_gravityDirection.normalized;
        public bool IsGravityEnable => !Mathf.Approximately(_gravityScale, 0f) && _gravityDirection.magnitude >= 0.05f;

        [SerializeField] private float _gravityScale = 1f;
        [SerializeField] private float _gravityStrength = 9.8f;
        [SerializeField] private float _groundCheckDistance = 0.1f;
        [SerializeField] private LayerMask _groundLayer = 6;

        private Rigidbody2D _rigidCompo;
        private Vector2 _moveVelocity;
        private Vector2 _gravityVelocity;
        private bool _isGrounded;

        public Vector2 Velocity => _rigidCompo.linearVelocity;
        public bool IsGrounded => _isGrounded;

        private void Awake()
        {
            _rigidCompo = GetComponent<Rigidbody2D>();
            _rigidCompo.gravityScale = 0f;
        }

        private void FixedUpdate()
        {
            GroundCheck();
            ApplyCustomGravity();
            Vector2 finalVelocity = _moveVelocity + _gravityVelocity;
            _rigidCompo.linearVelocity = finalVelocity;
        }

        #region Gravity
        private void ApplyCustomGravity()
        {
            if (!IsGravityEnable) return;

            if (_isGrounded)
            {
                AlignToGround();
            }
            else
            {
                _gravityVelocity += _gravityDirection.normalized * _gravityStrength * _gravityScale * Time.fixedDeltaTime;
            }
        }

        private void AlignToGround()
        {
            Vector2 gravityDir = _gravityDirection.normalized;
            float verticalSpeed = Vector2.Dot(_gravityVelocity, gravityDir);
            _gravityVelocity -= gravityDir * verticalSpeed;
        }

        private void GroundCheck()
        {
            Vector2 origin = (Vector2)transform.position;
            RaycastHit2D hit = Physics2D.Raycast(origin, _gravityDirection.normalized, _groundCheckDistance, _groundLayer);
            _isGrounded = hit.collider != null;
        }
        #endregion

        #region Movement
        public void SetMovement(Vector2 moveVelocity)
        {
            _moveVelocity = moveVelocity;
        }

        public void StopMovement()
        {
            _moveVelocity = Vector2.zero;
        }
        #endregion

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, _gravityDirection.normalized * _groundCheckDistance);
        }

        public void SetGravityDirection(Vector2 direction)
        {
            _gravityDirection = direction.normalized;
        }
#endif
    }
}
