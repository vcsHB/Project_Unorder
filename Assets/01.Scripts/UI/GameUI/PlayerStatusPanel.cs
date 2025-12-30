using System.Collections.Generic;
using Project_Unorder.AgentSystem.PlayerManage;
using UnityEngine;
namespace Project_Unorder.UIManage.InGameSceneUI
{

    public class PlayerStatusPanel : MonoBehaviour
    {
        [SerializeField] private PlayerStatusItem _playerStatusItemPrefab;
        [SerializeField] private Transform _statusContentTrm;

        private List<PlayerDataSO> _playerDataList = new();

        public void AddPlayer(PlayerDataSO playerData)
        {
            
        }
    }
}