using UnityEngine;

namespace Project_Unorder.AgentSystem.BossSystem.AttackPattern.Selection
{
    public abstract class PatternSelectorSO : ScriptableObject
    {
        public abstract int Select(BossPatternEntry[] entries, PatternSelectionState state);
    }
}
