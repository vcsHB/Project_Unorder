using UnityEngine;
namespace Project_Unorder.CombatSystem.CasterSystem
{

    public interface ICastable3D
    {
        public void Cast(Collider target);
    }
}