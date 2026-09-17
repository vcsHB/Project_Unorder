using UnityEngine;

namespace Project_Unorder.AgentSystem.BossSystem.AttackPattern.Selection
{
    [CreateAssetMenu(menuName = "SO/Boss/Selector/Random")]
    public class RandomPatternSelectorSO : PatternSelectorSO
    {
        [SerializeField] private bool _avoidImmediateRepeat = true;

        public override int Select(BossPatternEntry[] entries, PatternSelectionState state)
        {
            int count = entries.Length;
            bool canAvoidRepeat = _avoidImmediateRepeat && count > 1 && state.LastIndex >= 0;
            if (!canAvoidRepeat)
                return Random.Range(0, count);

            int index = Random.Range(0, count - 1);
            return index >= state.LastIndex ? index + 1 : index;
        }
    }
}
