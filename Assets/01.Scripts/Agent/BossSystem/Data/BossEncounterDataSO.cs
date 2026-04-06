using UnityEngine;

namespace Project_Unorder.AgentSystem.BossSystem.Data
{
    [CreateAssetMenu(menuName = "SO/Boss/EncounterData")]
    public class BossEncounterDataSO : ScriptableObject
    {
        public BossPhaseDataSO[] Phases;
    }
}
