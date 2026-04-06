using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

namespace Project_Unorder.AgentSystem.BossSystem.BT.ActionNodes
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "BossPhaseTransition", story: "[BossAgent] transition to next phase", category: "Boss/Action", id: "d4e5f6789012345678abcdef01020304")]
    public partial class BossPhaseTransitionAction : Action
    {
        [SerializeReference] public BlackboardVariable<Boss> BossAgent;
        [CreateProperty] private bool _isTransitionComplete;

        protected override Status OnStart()
        {
            Boss boss = BossAgent.Value;
            if (boss == null) return Status.Failure;
            if (!boss.EncounterController.HasNextPhase) return Status.Failure;

            _isTransitionComplete = false;
            boss.EncounterController.RequestPhaseTransition(() => _isTransitionComplete = true);
            return Status.Running;
        }

        protected override Status OnUpdate()
        {
            return _isTransitionComplete ? Status.Success : Status.Running;
        }
    }
}
