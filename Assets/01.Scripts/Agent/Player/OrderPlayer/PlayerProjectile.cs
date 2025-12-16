using UnityEngine;

namespace Project_Unorder.CombatSystem.ProjectileSystem
{
    public class PlayerProjectile : Projectile
    {
        public float OrbitEnterProgress { get; set; } = 0f;

        [SerializeField] private float _accelDuration = 0.25f;   // 0.25초 동안 가속
        private float _flyProgress = 0f;                        // 0->1

        private Transform _target;
        private bool _isFlying = false;

        void FixedUpdate()
        {
            if (!_isProjectileEnable || !_isFlying)
                return;

            if (_target == null)
            {
                Destroy();
                return;
            }

            // 방향 벡터
            Vector2 dir = (_target.position - transform.position).normalized;

            // 회전 처리
            _visualTrm.right = dir;

            // 가속 증가
            if (_flyProgress < 1f)
                _flyProgress += Time.fixedDeltaTime / _accelDuration;

            float accelFactor = Mathf.SmoothStep(0f, 1f, _flyProgress); // 부드러운 가속

            float speed = _data.speed * accelFactor;

            transform.position += (Vector3)(dir * speed * Time.fixedDeltaTime);
        }

        public override void Shoot(ProjectileData projectileData)
        {
            base.Shoot(projectileData);
        }

    }
}
