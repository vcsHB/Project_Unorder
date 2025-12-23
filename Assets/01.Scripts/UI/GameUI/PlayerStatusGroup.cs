using System.Collections;
using System.Collections.Generic;
using Project_Unorder.AgentSystem.PlayerManage;
using UnityEngine;

namespace Project_Unorder.UIManage.InGameSceneUI.MainUIs
{

    public class PlayerStatusGroup : MonoBehaviour
    {
        [SerializeField] private PlayerStatusItem _statusItemPrefab;
        [SerializeField] private Transform _contentTrm;
        [SerializeField] private float _initializeTerm = 0.2f;
        private List<PlayerStatusItem> _statusItemList = new();

        public void Initialize(PlayerDataSO[] players)
        {
            StartCoroutine(StatusItemInitializeRoutine(players));
        }

        private IEnumerator StatusItemInitializeRoutine(PlayerDataSO[] players)
        {
            for (int i = 0; i < players.Length; i++)
            {
                PlayerStatusItem statusItem = Instantiate(_statusItemPrefab, _contentTrm);
                statusItem.SetPlayerData(players[i]);
                _statusItemList.Add(statusItem);
                statusItem.Open();

                yield return new WaitForSeconds(_initializeTerm);
            }
        }

    }
}