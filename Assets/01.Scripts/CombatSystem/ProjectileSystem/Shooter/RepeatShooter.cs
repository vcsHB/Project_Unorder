using Project_Unorder.Core.Attribute;
using UnityEngine;
namespace Project_Unorder.CombatSystem.ProjectileSystem
{

    public class RepeatShooter : MonoBehaviour
    {
        [SerializeField] private ProjectileSO _projectileSO;

        [SerializeField] private bool _canFire;
        [SerializeField] private float _fireTerm = 0.4f;
        [SerializeField] private ProjectileData _projectileData;

        [SerializeField] private bool _usePositionRandomize;
        [SerializeField, ShowIf(nameof(_usePositionRandomize))] private float _randomizeRadius = 10f;
        [SerializeField, ShowIf(nameof(_usePositionRandomize))] private bool _onlyEdge; // Random Vector Normalize Setting
        private Transform _targetTrm;
        private float _nextFireTime;

        public void SetFireState(bool value)
        {
            _canFire = value;
        }
        public void SetTarget(Transform target)
        {
            _targetTrm = target;
        }

        private void Update()
        {
            if (!_canFire) return;

            if (Time.time > _nextFireTime)
            {
                // Fire
                _nextFireTime = Time.time + _fireTerm;
                Vector2 position = transform.position;
                if (_usePositionRandomize)
                {
                    position += Random.insideUnitCircle * _randomizeRadius;
                    if (_onlyEdge)
                    {
                        position.Normalize();
                    }
                }

                if (_targetTrm == null) return;
                Projectile projectile = _projectileSO.GetProjectile(position);
                projectile.Shoot(new ProjectileData()
                {
                    targetTrm = _targetTrm,
                    direction = (Vector2)_targetTrm.position - position,
                    speed = _projectileData.speed,
                    lifeTime = _projectileData.lifeTime
                });
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (_usePositionRandomize)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, _randomizeRadius);
            }
        }
    }
}