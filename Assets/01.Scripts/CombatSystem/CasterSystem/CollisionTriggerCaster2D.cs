using UnityEngine;
namespace Project_Unorder.CombatSystem.CasterSystem
{

    public class CollisionTriggerCaster2D : Caster2D
    {

        public override void Cast()
        {

        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            ForceCast(collision);
        }
    }
}