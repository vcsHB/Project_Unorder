using System;
using System.Collections;
using Project_Unorder.AgentSystem.BossSystem.Data;
using Project_Unorder.LogSystem;
using UnityEngine;

namespace Project_Unorder.AgentSystem.BossSystem
{
    public class BossEncounterController : MonoBehaviour
    {
        public event Action OnEncounterStartEvent;

        [SerializeField] private BossEncounterDataSO _encounterData;
        [SerializeField] private Boss _boss;
        [SerializeField] private DialoguePlayer _dialoguePlayer;

        private int _currentPhaseIndex;
        private bool _skipIntro;
        private bool _isStarted;

        public BossPhaseDataSO CurrentPhase => _encounterData.Phases[_currentPhaseIndex];
        public int CurrentPhaseIndex => _currentPhaseIndex;
        public bool HasNextPhase => _currentPhaseIndex + 1 < _encounterData.Phases.Length;
        public bool IsStarted => _isStarted;

        public void StartEncounter(bool skipIntro = false, int startPhaseIndex = 0)
        {
            if (_isStarted) return;

            if (_encounterData == null || _encounterData.Phases == null || _encounterData.Phases.Length == 0)
            {
                Debug.LogError("BossEncounterController: EncounterData has no phase");
                return;
            }

            if (startPhaseIndex < 0 || startPhaseIndex >= _encounterData.Phases.Length)
            {
                Debug.LogError($"BossEncounterController: StartPhaseIndex {startPhaseIndex} is out of range");
                return;
            }

            _isStarted = true;
            _skipIntro = skipIntro;
            _currentPhaseIndex = startPhaseIndex;

            OnEncounterStartEvent?.Invoke();
            StartCoroutine(EncounterSequence());
        }

        public void JumpToPhase(int phaseIndex)
        {
            if (_encounterData == null || phaseIndex < 0 || phaseIndex >= _encounterData.Phases.Length)
            {
                Debug.LogError($"BossEncounterController: PhaseIndex {phaseIndex} is out of range");
                return;
            }

            if (_boss == null)
            {
                Debug.LogError("BossEncounterController: Boss is not binded");
                return;
            }

            _currentPhaseIndex = phaseIndex;
            _boss.OnPhaseChanged();
        }

        private IEnumerator EncounterSequence()
        {
            yield return StartCoroutine(RunPhaseIntro(CurrentPhase));
            _boss.BeginBattle(this);
        }

        public void RequestPhaseTransition(Action onTransitionComplete)
        {
            StartCoroutine(PhaseTransitionSequence(onTransitionComplete));
        }

        private IEnumerator PhaseTransitionSequence(Action onComplete)
        {
            _currentPhaseIndex++;

            if (_currentPhaseIndex >= _encounterData.Phases.Length)
            {
                onComplete?.Invoke();
                yield break;
            }

            yield return StartCoroutine(RunPhaseIntro(CurrentPhase));
            _boss.OnPhaseChanged();
            onComplete?.Invoke();
        }

        private IEnumerator RunPhaseIntro(BossPhaseDataSO phase)
        {
            if (_skipIntro) yield break;

            if (phase.PrePhaseDialogue != null)
            {
                bool dialogueDone = false;
                _dialoguePlayer.Play(phase.PrePhaseDialogue, () => dialogueDone = true);
                yield return new WaitUntil(() => dialogueDone);
            }

            if (phase.IntroCinematic != null)
            {
                yield return StartCoroutine(phase.IntroCinematic.Play(this, _boss));
            }
        }
    }
}
