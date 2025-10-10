using UnityEngine;
namespace Project_Unorder.CombatSystem.CasterSystem
{

    public interface I2DCastable 
    {
        public void Cast(Collider2D target);
        
    }
}