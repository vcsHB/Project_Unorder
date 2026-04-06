using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

namespace Project_Unorder.AgentSystem.BossSystem.BT.ActionNodes
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "SelectAttackPattern", story: "[BossAgent] select pattern from current phase", category: "Boss/Action", id: "b2c3d4e5f6789012345678abcdef0102")]
    public partial class SelectAttackPatternAction : Action
    {
        [SerializeReference] public BlackboardVariable<Boss> BossAgent;

        protected override Status OnStart()
        {
            Boss boss = BossAgent.Value;
            if (boss == null) return Status.Failure;

            boss.SelectRandomPattern();
            return Status.Success;
        }
    }
}
