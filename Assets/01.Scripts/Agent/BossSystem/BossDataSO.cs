using UnityEngine;
namespace Project_Unorder.AgentSystem.BossSystem
{
    [CreateAssetMenu(menuName = "SO/BossData")]
    public class BossDataSO : ScriptableObject
    {
        public string bossName;
        public Boss bossPrefab;
        
    }
}