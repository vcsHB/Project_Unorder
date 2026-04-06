using System;
using System.Collections;
using Project_Unorder.AgentSystem.BossSystem.Data;
using Project_Unorder.LogSystem;
using UnityEngine;

namespace Project_Unorder.AgentSystem.BossSystem
{
    public class BossEncounterController : MonoBehaviour
    {
        [SerializeField] private BossEncounterDataSO _encounterData;
        [SerializeField] private Boss _boss;
        [SerializeField] private DialoguePlayer _dialoguePlayer;

        private int _currentPhaseIndex;

        public BossPhaseDataSO CurrentPhase => _encounterData.Phases[_currentPhaseIndex];
        public int CurrentPhaseIndex => _currentPhaseIndex;
        public bool HasNextPhase => _currentPhaseIndex + 1 < _encounterData.Phases.Length;

        private void Start()
        {
            StartEncounter();
        }

        private void StartEncounter()
        {
            StartCoroutine(EncounterSequence());
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
