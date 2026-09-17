using System;
using UnityEngine;

namespace Project_Unorder.AgentSystem.BossSystem.AttackPattern
{
    [Serializable]
    public class BossPatternEntry
    {
        public BossAttackPatternSO pattern;
        [Min(0f)] public float weight = 1f;
        [Min(0f)] public float delayAfter = 1f;
    }
}
