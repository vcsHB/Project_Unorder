using UnityEngine;
namespace Project_Unorder.CombatSystem.ProjectileSystem
{

    public class CircleProjectileShooter : ProjectileShooter
    {
        [SerializeField] private int _projectileAmount = 10;
        [SerializeField] private float _speed = 5f;
        [SerializeField] private float _lifeTime = 5f;
        [SerializeField] private float _damage = 8f;

        [ContextMenu("DebugFire")]
        public override void Fire()
        {
            float angleStep = 360f / _projectileAmount;

            for (int i = 0; i < _projectileAmount; i++)
            {
                // Calculate the current angle for this projectile
                float angle = i * angleStep;

                Vector2 direction = new Vector2(
                    Mathf.Cos(angle * Mathf.Deg2Rad),
                    Mathf.Sin(angle * Mathf.Deg2Rad)
                );

                Projectile projectile = GenerateProjectile();


                projectile.Shoot(new ProjectileData()
                {
                    direction = direction, // The calculated circular direction
                    speed = _speed,
                    lifeTime = _lifeTime,
                    damage = _damage
                });
            }
        }
    }
}