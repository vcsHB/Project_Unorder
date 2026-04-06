using Project_Unorder.AgentSystem;
using Project_Unorder.CombatSystem.ProjectileSystem;
using UnityEngine;

namespace Project_Unorder.AgentSystem.BossSystem.Shooter
{
    public class BossProjectileEmitter : MonoBehaviour, IAgentComponent
    {
        [SerializeField] private ProjectileSO[] _projectileTypes;

        private Boss _boss;

        public void Initialize(AgentSystem.Agent agent) => _boss = agent as Boss;
        public void AfterInitialize() { }
        public void LateInitialize() { }

        public void Emit(int typeIndex, Vector2 direction, float speed, float damage, float lifeTime, Transform target = null)
        {
            Projectile projectile = _projectileTypes[typeIndex].GetProjectile();
            projectile.transform.position = transform.position;
            projectile.Shoot(new ProjectileData
            {
                direction = direction,
                speed = speed,
                damage = damage,
                lifeTime = lifeTime,
                targetTrm = target
            });
        }

        public void EmitAtPosition(int typeIndex, Vector2 spawnPosition, Vector2 direction, float speed, float damage, float lifeTime, Transform target = null)
        {
            Projectile projectile = _projectileTypes[typeIndex].GetProjectile();
            projectile.transform.position = spawnPosition;
            projectile.Shoot(new ProjectileData
            {
                direction = direction,
                speed = speed,
                damage = damage,
                lifeTime = lifeTime,
                targetTrm = target
            });
        }

        public void Dispose()
        {
        }

    }
}
