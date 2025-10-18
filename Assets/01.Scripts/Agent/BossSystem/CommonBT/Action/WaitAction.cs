using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

namespace Project_Unorder.AgentSystem.BossSystem.BT.ActionNodes
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "Wait", story: "Wait for [SecondsToWait] Second", category: "Action", id: "fb4c1af901aa3c18b50460728b06e21e")]
    public partial class WaitAction : Action
    {
        [SerializeReference] public BlackboardVariable<float> SecondsToWait;
        [CreateProperty] private float _timer = 0.0f;

        protected override Status OnStart()
        {
            _timer = SecondsToWait;
            if (_timer <= 0.0f)
            {
                return Status.Success;
            }

            return Status.Running;
        }

        protected override Status OnUpdate()
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                return Status.Success;
            }

            return Status.Running;
        }
    }
}

