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
            if (((1 << collision.gameObject.layer) & _detectTargetLayer.value) != 0)
                ForceCast(collision);
        }
    }
}