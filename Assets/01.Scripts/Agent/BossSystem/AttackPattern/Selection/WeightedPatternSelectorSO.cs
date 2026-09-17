using UnityEngine;

namespace Project_Unorder.AgentSystem.BossSystem.AttackPattern.Selection
{
    [CreateAssetMenu(menuName = "SO/Boss/Selector/Weighted")]
    public class WeightedPatternSelectorSO : PatternSelectorSO
    {
        public override int Select(BossPatternEntry[] entries, PatternSelectionState state)
        {
            float totalWeight = 0f;
            for (int i = 0; i < entries.Length; i++)
                totalWeight += entries[i].weight;

            if (totalWeight <= 0f)
                return Random.Range(0, entries.Length);

            float roll = Random.Range(0f, totalWeight);
            for (int i = 0; i < entries.Length; i++)
            {
                roll -= entries[i].weight;
                if (roll < 0f)
                    return i;
            }
            return entries.Length - 1;
        }
    }
}
