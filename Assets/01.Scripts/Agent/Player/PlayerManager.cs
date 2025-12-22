using UnityEngine;
namespace Project_Unorder.AgentSystem.PlayerManage
{
    public class PlayerManager : MonoSingleton<PlayerManager>, IEarlyAwakeableManager
    {
        [SerializeField] private Player[] _players;

        public void PreAwake()
        {
            Debug.Assert(_players != null);
            // Debug.Assert(_players.Length == 2); //Just in case
            Debug.Log("[PlayerManager:PreAwake] Player Instance Registered.");
            for (int i = 0; i < _players.Length; i++)
            {
                if (_players[i] == null)
                {
                    Debug.LogError($"[PlayerManager] Register Target(index:{i}) is null");
                    continue;
                }
                _players[i].RegisterInstance();
            }
        }

        private void OnDestroy()
        {

        }
    }
}