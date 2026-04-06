using Project_Unorder.AgentSystem.BossSystem.AttackPattern;
using Project_Unorder.AgentSystem.BossSystem.Cinematic;
using Project_Unorder.LogSystem;
using UnityEngine;

namespace Project_Unorder.AgentSystem.BossSystem.Data
{
    [CreateAssetMenu(menuName = "SO/Boss/PhaseData")]
    public class BossPhaseDataSO : ScriptableObject
    {
        [Header("Intro")]
        public LogSequence PrePhaseDialogue;
        public BossCinematic IntroCinematic;

        [Header("Combat")]
        public BossAttackPatternSO[] Patterns;

        [Header("Transition")]
        [Range(0f, 1f)]
        public float NextPhaseHpThreshold;
    }
}
