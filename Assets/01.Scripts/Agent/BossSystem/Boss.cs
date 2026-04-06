using System.Collections;
using Project_Unorder.AgentSystem.BossSystem.AttackPattern;

using Unity.Behavior;
using UnityEngine;

namespace Project_Unorder.AgentSystem.BossSystem
{
    public class Boss : Agent
    {
        protected BehaviorGraphAgent _btAgent;

        public BossEncounterController EncounterController { get; private set; }
        public BossAttackPatternSO SelectedPattern { get; private set; }
        public bool IsPatternRunning { get; private set; }

        private Coroutine _patternCoroutine;

        protected override void Awake()
        {
            base.Awake();
            _btAgent = GetCompo<BehaviorGraphAgent>();
        }

        public void BeginBattle(BossEncounterController controller)
        {
            EncounterController = controller;
            OnPhaseChanged();
            _btAgent.enabled = true;
        }

        public void OnPhaseChanged()
        {
            SetVariable("CurrentPhaseIndex", EncounterController.CurrentPhaseIndex);
        }

        public void SelectRandomPattern()
        {
            BossAttackPatternSO[] patterns = EncounterController.CurrentPhase.Patterns;
            if (patterns == null || patterns.Length == 0) return;
            SelectedPattern = patterns[Random.Range(0, patterns.Length)];
        }

        public void ExecuteSelectedPattern(Transform playerTarget)
        {
            if (SelectedPattern == null || IsPatternRunning) return;
            _patternCoroutine = StartCoroutine(PatternRoutine(playerTarget));
        }

        public void StopCurrentPattern()
        {
            if (_patternCoroutine != null)
            {
                StopCoroutine(_patternCoroutine);
                _patternCoroutine = null;
            }
            IsPatternRunning = false;
        }

        private IEnumerator PatternRoutine(Transform playerTarget)
        {
            IsPatternRunning = true;
            BossAttackContext context = new BossAttackContext
            {
                Boss = this,
                PlayerTarget = playerTarget
            };
            yield return StartCoroutine(SelectedPattern.Execute(context));
            IsPatternRunning = false;
        }

        public BlackboardVariable<T> GetVariable<T>(string variableName)
        {
            if (_btAgent.GetVariable(variableName, out BlackboardVariable<T> variable))
                return variable;
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
