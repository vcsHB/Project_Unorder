using UnityEngine;
namespace Project_Unorder.AgentSystem.PlayerManage
{
    [CreateAssetMenu(menuName = "SO/PlayerData")]
    public class PlayerDataSO : ScriptableObject
    {
        public PlayerType playerType;
        public Color personalColor;

        
    }
}