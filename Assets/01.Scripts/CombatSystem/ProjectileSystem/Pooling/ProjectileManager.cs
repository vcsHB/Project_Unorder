using UnityEngine;
namespace Project_Unorder.CombatSystem.ProjectileSystem
{

    public class ProjectileManager : MonoBehaviour
    {
        private ProjectileSO[] _enabledProjectiles;
        
        public void EnableProjectile(ProjectileSO[] projectileData)
        {
            _enabledProjectiles = projectileData;
            for (int i = 0; i < projectileData.Length; i++)
            {
                projectileData[i].SetProjectileEnable();
            }
        }

        public void DisableProjectiles()
        {
            for (int i = 0; i < _enabledProjectiles.Length; i++)
            {
                _enabledProjectiles[i].SetProjectileDisable();
            }
        }



    }
}