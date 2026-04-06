using System.Collections;
using UnityEngine;

namespace Project_Unorder.AgentSystem.BossSystem.AttackPattern
{
    public abstract class BossAttackPatternSO : ScriptableObject
    {
        public BulletPatternType PatternType;
        public float Cooldown;

        public abstract IEnumerator Execute(BossAttackContext context);
    }
}
