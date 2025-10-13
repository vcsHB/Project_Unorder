using System.Collections.Generic;
using UnityEngine;
namespace Project_Unorder.CombatSystem.ProjectileSystem
{
    [CreateAssetMenu(menuName = "SO/ProjectileSO")]
    public class ProjectileSO : ScriptableObject
    {
        public string projectileName;
        public Projectile projectilePrefab;
        public int poolAmount;
        public Stack<Projectile> pool;

        #region External Functions
        public Projectile GetProjectile()
        {
            Projectile projectile = pool.Count > 0 ? pool.Pop() : CreateProjectile();
            projectile.gameObject.SetActive(true);
            return projectile;
        }
        public void SetProjectileEnable()
        {
            if (pool == null)
                pool = new();

            if (projectilePrefab == null)
            {
                Debug.LogError("[Projectile Pooling] ProjectilePrefab is not binded");
                return;
            }

            pool.Clear();
            for (int i = 0; i < poolAmount; i++)
            {
                Projectile projectile = CreateProjectile();
                pool.Push(projectile);
            }
        }

        public void SetProjectileDisable()
        {
            pool.Clear();
        }

        #endregion


        private Projectile CreateProjectile()
        {
            Projectile projectile = GameObject.Instantiate(projectilePrefab);
            projectile.OnProjectileDestroyEvent += HandleProjectileDestroy;
            projectile.gameObject.SetActive(false);
            return projectile;
        }

        private void HandleProjectileDestroy(Projectile projectile)
        {
            pool.Push(projectile);
            projectile.gameObject.SetActive(false);
        }


    }
}