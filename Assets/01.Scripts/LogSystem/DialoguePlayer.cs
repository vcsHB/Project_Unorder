using System;
using System.Collections;
using UnityEngine;

namespace Project_Unorder.LogSystem
{
    public class DialoguePlayer : MonoBehaviour
    {
        [SerializeField] private float _intervalPerLog = 2f;

        private Coroutine _playRoutine;

        public void Play(LogSequence sequence, Action onComplete = null)
        {
            if (_playRoutine != null)
                StopCoroutine(_playRoutine);
            _playRoutine = StartCoroutine(PlaySequence(sequence, onComplete));
        }

        public void Stop()
        {
            if (_playRoutine != null)
            {
                StopCoroutine(_playRoutine);
                _playRoutine = null;
            }
        }

        private IEnumerator PlaySequence(LogSequence sequence, Action onComplete)
        {
            for (int i = 0; i < sequence.logs.Length; i++)
            {
                LogReceiveChannel.Broadcast(sequence.logs[i]);
                yield return new WaitForSeconds(_intervalPerLog);
            }
            _playRoutine = null;
            onComplete?.Invoke();
        }
    }
}
