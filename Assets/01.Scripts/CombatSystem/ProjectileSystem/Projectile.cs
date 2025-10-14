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
        protected float _destroyTime;
        protected bool _isProjectileEnable;
        
        protected virtual void Awake()
        {
            _rigidCompo = GetComponent<Rigidbody2D>();
            _caster.OnCastSuccessEvent.AddListener(Destroy);
        }


        public virtual void Shoot(ProjectileData projectileData)
        {
            _destroyTime = projectileData.lifeTime + Time.time;
            _isProjectileEnable = true;
        }

        protected virtual void Update()
        {
            if (_isProjectileEnable)
            {

                if (_destroyTime < Time.time)
                {
                    Destroy();
                }
            }
        }

        protected virtual void Destroy()
        {
            _isProjectileEnable = false;
            OnProjectileDestroyEvent?.Invoke(this);
        }
    }
}