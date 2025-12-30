using UnityEngine;
namespace Project_Unorder.AgentSystem.PlayerManage
{

    public class OrderPlayerAttacker : MonoBehaviour, IAgentComponent
    {
        [SerializeField] private LayerMask _targetLayer;
        [SerializeField] private float _detectRadius = 10f;
        private Collider2D _targetCollider;
        private ProjectileController _projectileController;

        private void Awake()
        {
            _projectileController = GetComponentInChildren<ProjectileController>();
        }

        private void DetectTarget()
        {
            _targetCollider = Physics2D.OverlapCircle(transform.position, _detectRadius, _targetLayer);
        }

        public void HandleAttack()
        {
            DetectTarget();
            if (_targetCollider == null) return;
            _projectileController.TryShoot(_targetCollider.transform);
        }

        public void Initialize(Agent owner)
        {
            
        }

        public void AfterInitialize()
        {
        }

        public void LateInitialize()
        {
        }

        public void Dispose()
        {
        }
    }
}