using InputManage;
using UnityEngine;
namespace Project_Unorder.AgentSystem.PlayerManage
{
    public class Player : Agent
    {
        [field: SerializeField] public PlayerDataSO PlayerData;
        [field: SerializeField] public PlayerInput PlayerInput { get; private set; }

        protected override void Awake()
        {
            PlayerData.RegisterPlayerInstance(this);
            base.Awake();
        }

        protected virtual void OnDestroy()
        {
            PlayerData.ReleasePlayereInstance();
        }

    }
}