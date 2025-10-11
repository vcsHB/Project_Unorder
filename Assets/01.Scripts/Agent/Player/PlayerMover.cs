using UnityEngine;
namespace Project_Unorder.AgentSytstem.PlayerManage
{

    public class PlayerMover : AgentMover
    {
        [SerializeField] private PlayerVisualRotator _visualRotator;


        public override void SetVelocity(Vector2 velocity)
        {
            base.SetVelocity(velocity);
            _physicsCompo.SetVelocity(velocity);
        }
    }
}