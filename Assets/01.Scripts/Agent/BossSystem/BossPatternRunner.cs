using System;
using System.Collections;
using Project_Unorder.AgentSystem.BossSystem.AttackPattern;
using Project_Unorder.AgentSystem.BossSystem.AttackPattern.Selection;
using Project_Unorder.AgentSystem.BossSystem.Data;
using Project_Unorder.AgentSystem.PlayerManage;
using UnityEngine;

namespace Project_Unorder.AgentSystem.BossSystem
{
    public class BossPatternRunner : MonoBehaviour, IAgentComponent
    {
        public event Action<int> OnPhaseEnterEvent;
        public event Action<BossAttackPatternSO> OnPatternStartEvent;
        public event Action<BossAttackPatternSO> OnPatternEndEvent;

        [SerializeField] private PlayerDataSO _targetPlayerData;
        [SerializeField, Min(0f)] private float _startDelay = 1f;

        private readonly PatternSelectionState _selectionState = new PatternSelectionState();
        private Boss _boss;
        private BossEncounterController _encounter;
        private Coroutine _loopCoroutine;
        private Coroutine _patternCoroutine;
        private BossAttackPatternSO _requestedPattern;
        private bool _isPaused;

        public bool IsRunning => _loopCoroutine != null;
        public bool IsPaused => _isPaused;
        public BossAttackPatternSO CurrentPattern { get; private set; }

        public void Initialize(Agent owner)
        {
            _boss = owner as Boss;
        }

        public void AfterInitialize()
        {
            if (_boss.HealthBody != null)
                _boss.HealthBody.OnDieEvent.AddListener(StopRunning);
        }

        public void LateInitialize()
        {
        }

        public void Dispose()
        {
            StopRunning();
            if (_boss != null && _boss.HealthBody != null)
                _boss.HealthBody.OnDieEvent.RemoveListener(StopRunning);
        }

        private void OnDestroy()
        {
            Dispose();
        }

        public void StartRunning(BossEncounterController encounter)
        {
            if (IsRunning) return;

            _encounter = encounter;
            _selectionState.Reset();
            _loopCoroutine = StartCoroutine(RunLoop());
        }

        public void StopRunning()
        {
            if (_patternCoroutine != null)
            {
                StopCoroutine(_patternCoroutine);
                _patternCoroutine = null;
            }

            if (_loopCoroutine != null)
            {
                StopCoroutine(_loopCoroutine);
                _loopCoroutine = null;
            }

            CurrentPattern = null;
        }

        public void SetPaused(bool isPaused)
        {
            _isPaused = isPaused;
        }

        public void RequestPattern(BossAttackPatternSO pattern)
        {
            _requestedPattern = pattern;
        }

        public void NotifyPhaseChanged()
        {
            if (_encounter == null) return;

            _selectionState.Reset();
            OnPhaseEnterEvent?.Invoke(_encounter.CurrentPhaseIndex);
        }

        private IEnumerator RunLoop()
        {
            if (_startDelay > 0f)
                yield return new WaitForSeconds(_startDelay);

            OnPhaseEnterEvent?.Invoke(_encounter.CurrentPhaseIndex);

            while (true)
            {
                while (_isPaused)
                    yield return null;

                if (ShouldTransitionPhase())
                {
                    bool isTransitionDone = false;
                    _encounter.RequestPhaseTransition(() => isTransitionDone = true);
                    yield return new WaitUntil(() => isTransitionDone);
                    continue;
                }

                if (!TryPickPattern(_encounter.CurrentPhase, out BossAttackPatternSO pattern, out float delayAfter))
                {
                    Debug.LogError($"BossPatternRunner: Phase {_encounter.CurrentPhaseIndex} has no runnable pattern. Runner stopped");
                    _loopCoroutine = null;
                    yield break;
                }

                yield return ExecutePattern(pattern);

                if (delayAfter > 0f)
                    yield return new WaitForSeconds(delayAfter);
            }
        }

        private bool TryPickPattern(BossPhaseDataSO phase, out BossAttackPatternSO pattern, out float delayAfter)
        {
            pattern = null;
            delayAfter = 0f;

            BossPatternEntry[] entries = phase.Patterns;
            if (entries == null || entries.Length == 0) return false;

            if (_requestedPattern != null)
            {
                pattern = _requestedPattern;
                delayAfter = FindDelayAfter(entries, _requestedPattern);
                _requestedPattern = null;
                return true;
            }

            int index = phase.Selector != null
                ? phase.Selector.Select(entries, _selectionState)
                : (_selectionState.LastIndex + 1) % entries.Length;

            if (index < 0 || index >= entries.Length || entries[index].pattern == null) return false;

            _selectionState.Record(index);
            pattern = entries[index].pattern;
            delayAfter = entries[index].delayAfter;
            return true;
        }

        private float FindDelayAfter(BossPatternEntry[] entries, BossAttackPatternSO pattern)
        {
            for (int i = 0; i < entries.Length; i++)
            {
                if (entries[i].pattern == pattern)
                    return entries[i].delayAfter;
            }
            return 1f;
        }

        private IEnumerator ExecutePattern(BossAttackPatternSO pattern)
        {
            Transform target = ResolveTarget();
            if (target == null)
            {
                Debug.LogError("BossPatternRunner: Target player is not registered. Pattern skipped");
                yield return null;
                yield break;
            }

            CurrentPattern = pattern;
            OnPatternStartEvent?.Invoke(pattern);

            BossAttackContext context = new BossAttackContext
            {
                Boss = _boss,
                PlayerTarget = target
            };
            _patternCoroutine = StartCoroutine(pattern.Execute(context));
            yield return _patternCoroutine;
            _patternCoroutine = null;

            OnPatternEndEvent?.Invoke(pattern);
            CurrentPattern = null;
        }

        private bool ShouldTransitionPhase()
        {
            if (!_encounter.HasNextPhase) return false;

            var health = _boss.HealthBody;
            if (health == null || health.MaxHealth <= 0f) return false;

            return health.CurrentHealth / health.MaxHealth <= _encounter.CurrentPhase.NextPhaseHpThreshold;
        }

        private Transform ResolveTarget()
        {
            if (_targetPlayerData == null || !_targetPlayerData.IsPlayerRegistered) return null;
            return _targetPlayerData.PlayerInstance.transform;
        }
    }
}
