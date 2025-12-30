using UnityEngine;
namespace Project_Unorder.AgentSystem.PlayerManage
{
    [CreateAssetMenu(menuName = "SO/PlayerManage/PlayerData")]
    public class PlayerDataSO : ScriptableObject
    {
        public uint id => (uint)playerType;
        public PlayerType playerType;
        public Color personalColor;
        [System.NonSerialized] private Player _playerInstance;

        public Player PlayerInstance
        {
            get
            {
                if (_playerInstance == null)
                {
                    Debug.LogError("[Error] Player Instance is not Registered");
                    return null;
                }
                return _playerInstance;
            }
        }

        public bool IsPlayerRegistered => _playerInstance != null;


        public void RegisterPlayerInstance(Player playerInstance)
        {
            if (playerInstance == null)
            {
                Debug.LogError("New Register Player Instance is null");
                return;
            }
            _playerInstance = playerInstance;
        }
        public void ReleasePlayereInstance()
        {
            if (PlayerInstance == null)
            {
                Debug.LogWarning("Already Player Instance was Released");
                return;
            }
            _playerInstance = null;
        }
    }
}