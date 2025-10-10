using UnityEngine;
namespace Project_Unorder.CombatSystem
{
    
    public interface IDamageable
    {
        public DamageResponse ApplyDamage(DamageData damageData);
    }
}