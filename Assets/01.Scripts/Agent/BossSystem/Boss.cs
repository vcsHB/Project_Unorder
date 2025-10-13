using Unity.Behavior;
using UnityEngine;
namespace Project_Unorder.AgentSystem.BossSystem
{

    public class Boss : Agent
    {
        protected BehaviorGraphAgent _btAgent;
        protected override void Awake()
        {
            base.Awake();
            _btAgent = GetCompo<BehaviorGraphAgent>();
        }
        
        public BlackboardVariable<T> GetVariable<T>(string variableName)
        {
            if (_btAgent.GetVariable(variableName, out BlackboardVariable<T> variable))
            {
                return variable;
            }
            return null;
        }

        public void SetVariable<T>(string variableName, T value)
        {
            BlackboardVariable<T> variable = GetVariable<T>(variableName);
            Debug.Assert(variable != null, $"Variable {variableName} not found");
            variable.Value = value;
        }
    }
}