using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;

namespace Project_Unorder.AgentSystem.BossSystem.BT.ConditionNodes
{
    [Serializable, GeneratePropertyBag]
    [Condition(name: "BossHealthBelowThreshold", story: "[BossAgent] hp ratio below phase threshold", category: "Boss/Condition", id: "a1b2c3d4e5f6789012345678abcdef01")]
    public partial class HealthBelowThresholdCondition : Condition
    {
        [SerializeReference] public BlackboardVariable<Boss> BossAgent;

        public override bool IsTrue()
        {
            Boss boss = BossAgent.Value;
            if (boss == null || boss.HealthBody == null) return false;
            if (!boss.EncounterController.HasNextPhase) return false;

            float hpRatio = boss.HealthBody.CurrentHealth / boss.HealthBody.MaxHealth;
            float threshold = boss.EncounterController.CurrentPhase.NextPhaseHpThreshold;
            return hpRatio <= threshold;
        }
    }
}
