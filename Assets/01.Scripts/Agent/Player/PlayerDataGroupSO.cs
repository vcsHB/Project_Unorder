using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
namespace Project_Unorder.AgentSystem.PlayerManage
{
    [CreateAssetMenu(menuName = "SO/PlayerManage/PlayerDataGroup")]
    public class PlayerDataGroupSO : ScriptableObject
    {
        public PlayerDataSO[] players;

        public PlayerDataSO[] GetRegisteredPlayers()
        {
            List<PlayerDataSO> registeredDataList = new();
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i].IsPlayerRegistered)
                    registeredDataList.Add(players[i]);
            }
            return registeredDataList.ToArray();
        }
    }
}