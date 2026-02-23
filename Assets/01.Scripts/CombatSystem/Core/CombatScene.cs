using Project_Unorder.AgentSystem.BossSystem;
using UnityEngine;
namespace Project_Unorder.CombatSystem.Core
{
    [CreateAssetMenu(menuName ="SO/CombatScene")]
    public class CombatScene : ScriptableObject
    {
        public uint id;
        public BossDataSO bossData;
        public CombatSceneConfig condfig;


    }
}