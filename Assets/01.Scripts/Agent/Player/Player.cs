using InputManage;
using UnityEngine;
namespace Project_Unorder.AgentSystem.PlayerManage
{
    public class Player : Agent
    {
        [field: SerializeField] public PlayerDataSO PlayerData;
        [field: SerializeField] public PlayerInput PlayerInput { get; private set; }

        public void RegisterInstance()
        {
            PlayerData.RegisterPlayerInstance(this); 
            
        }
        protected override void Awake()
        {
            base.Awake();
        }
        public void SetPlayerActive(bool value)
        {
            gameObject.SetActive(value);
        }
        
        protected virtual void OnDestroy()
        {
            PlayerData.ReleasePlayereInstance();
        }

    }
}