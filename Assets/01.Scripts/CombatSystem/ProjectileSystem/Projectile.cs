using System;
using Project_Unorder.CombatSystem.CasterSystem;
using UnityEngine;
namespace Project_Unorder.CombatSystem.ProjectileSystem
{

    public abstract class Projectile : MonoBehaviour
    {
        public event Action<Projectile> OnProjectileDestroyEvent;
        protected Rigidbody2D _rigidCompo;
        [SerializeField] private CollisionTriggerCaster2D _caster;
        
        protected virtual void Awake()
        {
            _rigidCompo = GetComponent<Rigidbody2D>();
        }


        public virtual void Shoot(ProjectileData projectileData)
        {

        }

        protected virtual void Destroy()
        {
            OnProjectileDestroyEvent?.Invoke(this);
        }
    }
}