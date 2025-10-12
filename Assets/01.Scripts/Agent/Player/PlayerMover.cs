using UnityEngine;
namespace Project_Unorder.AgentSystem.PlayerManage
{

    public class PlayerMover : AgentMover
    {
        [SerializeField] private PlayerVisualRotator _visualRotator;


        public override void SetMovement(Vector2 direction)
        {
            base.SetMovement(direction);
            _visualRotator.SetDirection(direction);
        }
    }
}