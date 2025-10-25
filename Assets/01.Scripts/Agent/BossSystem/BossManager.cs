using UnityEngine;
namespace Project_Unorder.AgentSystem.BossSystem
{

    public class BossManager : MonoBehaviour
    {
        [SerializeField] private Boss _currentBoss;

        public void SetBoss(BossDataSO bossData)
        {
            _currentBoss = Instantiate(bossData.bossPrefab);
            
        }

    }
}