using UnityEngine;

namespace Project_Unorder.CombatSystem.ProjectileSystem
{
    public class PlayerProjectile : Projectile
    {
        public float OrbitEnterProgress { get; set; }

        [SerializeField] private float _accelDuration = 0.25f;
        [SerializeField] private float _curveStrength = 1.5f;

        private float _flyProgress;
        private Transform _target;
        private bool _isFlying;

        private float _curveSign;
        private float _curveFrequency;

        private void FixedUpdate()
        {
            if (!_isProjectileEnable || !_isFlying)
                return;

            if (_target == null)
            {
                Destroy();
                return;
            }

            Vector2 toTarget = _target.position - transform.position;
            Vector2 dir = toTarget.normalized;

            Vector2 perpendicular = new Vector2(-dir.y, dir.x);

            _visualTrm.up = dir;

            if (_flyProgress < 1f)
                _flyProgress += Time.fixedDeltaTime / _accelDuration;

            float accelFactor = Mathf.SmoothStep(0f, 1f, _flyProgress);
            float speed = _data.speed * accelFactor;

            float curve = Mathf.Sin(Time.time * _curveFrequency) * _curveStrength * _curveSign;

            Vector2 moveDir = (dir + perpendicular * curve).normalized;
            transform.position += (Vector3)(moveDir * speed * Time.fixedDeltaTime);
        }

        protected override void Destroy()
        {
            _isFlying = false;
            base.Destroy();
        }

        public override void Shoot(ProjectileData projectileData)
        {
            base.Shoot(projectileData);

            _target = _data.targetTrm;
            _isFlying = true;
            _flyProgress = 0f;

            _curveSign = Random.value < 0.5f ? -1f : 1f;
            _curveFrequency = Random.Range(3f, 6f);
        }
    }
}
