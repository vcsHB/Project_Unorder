using System;
using System.Collections;
using System.Collections.Generic;
using Project_Unorder.CombatSystem.ProjectileSystem;
using UnityEngine;

namespace Project_Unorder.AgentSystem.PlayerManage
{
    [Serializable]
    public class OrbitSlot
    {
        public PlayerProjectile projectile;
        public float currentAngle;
        public float targetAngle;
        public float radius;
    }

    public class ProjectileController : MonoBehaviour
    {
        [Header("Projectile Manage")]
        [SerializeField] private ProjectileSO _projectilePool;
        [SerializeField] private List<PlayerProjectile> _activatedProjectileList;

        [Header("Enemy Detect")]
        [SerializeField] private float _enemyDetectRadius = 5f;
        [SerializeField] private LayerMask _enemyLayerMask;
        private Collider2D[] _enemyDetecteds;
        private int _maxEnemyDetectAmount = 10;

        [Header("Projectile Status")]
        [SerializeField] private float _damage = 1f;
        [SerializeField] private float _projectileGenerateCooltime = 0.5f;
        private float _currentProjectileCooltime;

        [Header("Orbit Settings")]
        [SerializeField] private float _orbitRadius = 1.5f;
        [SerializeField] private float _orbitSpeed = 2f;
        [SerializeField] private float _orbitEnterSpeed = 3f;
        [SerializeField] private float _angleAlignSpeed = 4f;

        [Header("Shoot Settings")]
        [SerializeField] private float _shootInterval = 0.3f;

        private readonly List<OrbitSlot> _orbitSlots = new();
        private uint _waitOrderAmount;
        private bool _isShooting;

        private void Awake()
        {
            _enemyDetecteds = new Collider2D[_maxEnemyDetectAmount];
            if (_projectilePool == null)
                Debug.LogError("PlayerProjectile Pool is not Binded");

            _projectilePool.SetProjectileEnable();
        }

        private void OnDestroy()
        {
            _projectilePool.SetProjectileDisable();
        }

        private void Update()
        {
            if (_waitOrderAmount > 0)
            {
                _currentProjectileCooltime += Time.deltaTime;
                if (_currentProjectileCooltime >= _projectileGenerateCooltime)
                {
                    ChargeSingleProjectile();
                    _waitOrderAmount--;
                    _currentProjectileCooltime = 0f;
                }
            }

            UpdateOrbitProjectiles();
        }

        private PlayerProjectile GetProjectile()
        {
            return _projectilePool.GetProjectile() as PlayerProjectile;
        }

        private void ChargeSingleProjectile()
        {
            PlayerProjectile projectile = GetProjectile();
            projectile.transform.position = transform.position;
            projectile.OrbitEnterProgress = 0f;

            OrbitSlot slot = new OrbitSlot
            {
                projectile = projectile,
                currentAngle = _orbitSlots.Count > 0 ? _orbitSlots[^1].targetAngle : 0f,
                targetAngle = 0f,
                radius = 0f
            };

            _orbitSlots.Add(slot);
            _activatedProjectileList.Add(projectile);

            RecalculateTargetAngles();
        }

        private void RecalculateTargetAngles()
        {
            int amount = _orbitSlots.Count;
            if (amount == 0) return;

            float spacing = 360f / amount;
            for (int i = 0; i < amount; i++)
                _orbitSlots[i].targetAngle = i * spacing;
        }

        private void UpdateOrbitProjectiles()
        {
            if (_orbitSlots.Count == 0) return;

            Vector3 center = transform.position;
            float delta = Time.deltaTime;

            foreach (OrbitSlot slot in _orbitSlots)
            {
                if (slot.projectile == null) continue;

                slot.radius = Mathf.Lerp(slot.radius, _orbitRadius, delta * _orbitEnterSpeed);
                slot.currentAngle = Mathf.LerpAngle(slot.currentAngle, slot.targetAngle, delta * _angleAlignSpeed);

                float dynamicAngle = slot.currentAngle + Time.time * _orbitSpeed;
                float rad = dynamicAngle * Mathf.Deg2Rad;

                Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad)) * slot.radius;
                slot.projectile.transform.position = center + offset;
            }
        }

        public void TryShoot(Transform target)
        {
            if (_isShooting) return;
            if (_orbitSlots.Count == 0) return;

            StartCoroutine(ShootAllCoroutine(target));
        }

        private IEnumerator ShootAllCoroutine(Transform target)
        {
            _isShooting = true;

            while (_orbitSlots.Count > 0)
            {
                OrbitSlot slot = _orbitSlots[0];
                PlayerProjectile projectile = slot.projectile;

                _orbitSlots.RemoveAt(0);
                _activatedProjectileList.Remove(projectile);

                projectile.Shoot(new ProjectileData
                {
                    targetTrm = target
                });

                RecalculateTargetAngles();
                yield return new WaitForSeconds(_shootInterval);
            }

            _isShooting = false;
        }

        public void ChargeProjectile(uint amount)
        {
            _waitOrderAmount += amount;
        }
    }
}
