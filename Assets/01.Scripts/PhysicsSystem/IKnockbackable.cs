using UnityEngine;
namespace Project_Unorder.PhysicsSystem
{

    public interface IKnockbackable
    {
        public void ApplyKnockback(Vector2 direction, float power);

    }
}