using Project_Unorder.AgentSystem.BossSystem;
using UnityEngine;
namespace Project_Unorder.CombatSystem.Core
{

    public class CombatManager : MonoBehaviour, IEarlyAwakeableManager
    {
        // 
        [SerializeField] private BossDataSO _bossData;

        public void PreAwake()
        {

        }

        public void StartCombat()
        {
            
        }

        public void SetBossData(BossDataSO data)
        {
            _bossData = data;
            
        }


    }
}