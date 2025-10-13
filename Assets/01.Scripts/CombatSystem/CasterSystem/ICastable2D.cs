using UnityEngine;
namespace Project_Unorder.CombatSystem.CasterSystem
{

    public interface ICastable2D 
    {
        public void Cast(Collider2D target);
        
    }
}