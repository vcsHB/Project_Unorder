using System.Data.Common;
using UnityEngine;
namespace Project_Unorder.CombatSystem.ProjectileSystem
{

    public abstract class ProjectileShooter : MonoBehaviour
    {
        [SerializeField] private ProjectileSO _projectileData;
        public abstract void Fire();

        protected Projectile GenerateProjectile()
        {
            // Pooling
            Projectile projectile = _projectileData.GetProjectile();
            projectile.transform.position = transform.position;
            return projectile;
        }

    }
}