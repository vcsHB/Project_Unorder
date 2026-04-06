using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

namespace Project_Unorder.AgentSystem.BossSystem.BT.ActionNodes
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "ExecuteAttackPattern", story: "[BossAgent] execute selected pattern toward [PlayerTarget]", category: "Boss/Action", id: "c3d4e5f6789012345678abcdef010203")]
    public partial class ExecuteAttackPatternAction : Action
    {
        [SerializeReference] public BlackboardVariable<Boss> BossAgent;
        [SerializeReference] public BlackboardVariable<Transform> PlayerTarget;

        protected override Status OnStart()
        {
            Boss boss = BossAgent.Value;
            if (boss == null || boss.SelectedPattern == null) return Status.Failure;

            boss.ExecuteSelectedPattern(PlayerTarget.Value);
            return Status.Running;
        }

        protected override Status OnUpdate()
        {
            return BossAgent.Value.IsPatternRunning ? Status.Running : Status.Success;
        }

        protected override void OnEnd()
        {
            BossAgent.Value?.StopCurrentPattern();
        }
    }
}
