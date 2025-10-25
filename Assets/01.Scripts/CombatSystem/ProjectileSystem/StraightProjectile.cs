using UnityEngine;
namespace Project_Unorder.CombatSystem.ProjectileSystem
{

    public class StraightProjectile : Projectile
    {
        public override void Shoot(ProjectileData projectileData)
        {
            base.Shoot(projectileData);
            _rigidCompo.linearVelocity = projectileData.direction.normalized * projectileData.speed;
            
        }
    }
}