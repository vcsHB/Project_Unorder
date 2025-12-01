using UnityEngine;
namespace Project_Unorder.AgentSystem.PlayerManage
{

    public class UnorderPlayerMover : MonoBehaviour, IAgentComponent
    {
        private UnorderPlayer _owner;
        public void AfterInitialize()
        {
        }

        public void Dispose()
        {
        }

        public void Initialize(Agent owner)
        {
            _owner = owner as UnorderPlayer;
        }

        public void LateInitialize()
        {
        }

        private void FixedUpdate()
        {

        }
    }
}