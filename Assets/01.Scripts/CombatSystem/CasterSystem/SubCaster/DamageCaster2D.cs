using Project_Unorder.CombatSystem.CasterSystem;
using UnityEngine;
namespace Project_Unorder.CombatSystem.ProjectileSystem
{

    public class DamageCaster2D : MonoBehaviour, ICastable2D
    {

        [SerializeField] private float _damage;

        public void Cast(Collider2D target)
        {
            if (target.transform.TryGetComponent(out IDamageable damageable))
            {
                damageable.ApplyDamage(new DamageData()
                {
                    damage = _damage
                });
            }
        }

        public void SetDamage(float damage)
        {
            _damage = damage;
        }
    }
}