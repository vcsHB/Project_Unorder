using UnityEngine;
namespace Project_Unorder.CombatSystem.ProjectileSystem
{
    [System.Serializable]
    public struct ProjectileData
    {
        public Vector2 direction;
        public float speed;
        public float lifeTime;
        
        public float damage;
    }
}