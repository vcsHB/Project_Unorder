using UnityEngine;

namespace Project_Unorder.AgentSystem.BossSystem.AttackPattern.Selection
{
    [CreateAssetMenu(menuName = "SO/Boss/Selector/Sequential")]
    public class SequentialPatternSelectorSO : PatternSelectorSO
    {
        public override int Select(BossPatternEntry[] entries, PatternSelectionState state)
        {
            return (state.LastIndex + 1) % entries.Length;
        }
    }
}
