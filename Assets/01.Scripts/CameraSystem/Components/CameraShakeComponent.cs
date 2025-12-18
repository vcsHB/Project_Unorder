using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

namespace Project_Unorder.CameraSystem
{
    struct ShakeRequest
    {
        public float amplitude;
        public float time;
    }

    public class CameraShakeComponent : CameraComponentBase
    {
        private CinemachineBasicMultiChannelPerlin _noiseCompo;

        private Coroutine _shakeRoutine;
        private ShakeRequest? _currentShake;
        private readonly List<ShakeRequest> _pendingShakeSequenceList = new();

        public override void Initialize(CameraManager manager)
        {
            base.Initialize(manager);   
            _noiseCompo = manager.MainCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();
        }

        public void Shake(float amplitude, float duration)
        {
            // 1. Check Shake Sequence
            // 2. Compare
            // 3. DO 

            var newShake = new ShakeRequest
            {
                amplitude = amplitude,
                time = duration
            };

            if (_currentShake == null)
            {
                StartShake(newShake);
                return;
            }

            if (amplitude > _currentShake.Value.amplitude)
            {
                float remainTime = _currentShake.Value.time;
                _pendingShakeSequenceList.Add(new ShakeRequest
                {
                    amplitude = _currentShake.Value.amplitude,
                    time = remainTime
                });

                StartShake(newShake);
            }
            else
            {
                _pendingShakeSequenceList.Add(newShake);
            }
        }

        private void StartShake(ShakeRequest request)
        {
            if (_shakeRoutine != null)
                StopCoroutine(_shakeRoutine);

            _currentShake = request;
            _shakeRoutine = StartCoroutine(ShakeRoutine());
        }

        private IEnumerator ShakeRoutine()
        {
            _noiseCompo.AmplitudeGain = _currentShake.Value.amplitude;

            float time = _currentShake.Value.time;
            while (time > 0f)
            {
                time -= Time.deltaTime;
                _currentShake = new ShakeRequest
                {
                    amplitude = _currentShake.Value.amplitude,
                    time = time
                };
                yield return null;
            }

            _noiseCompo.AmplitudeGain = 0f;
            _currentShake = null;
            _shakeRoutine = null;

            PlayNextShake();
        }

        private void PlayNextShake()
        {
            if (_pendingShakeSequenceList.Count == 0)
                return;

            // Strongest Shaking in Sequence
            int strongestIndex = 0;
            for (int i = 1; i < _pendingShakeSequenceList.Count; i++)
            {
                if (_pendingShakeSequenceList[i].amplitude > _pendingShakeSequenceList[strongestIndex].amplitude)
                    strongestIndex = i;
            }

            var next = _pendingShakeSequenceList[strongestIndex];
            _pendingShakeSequenceList.RemoveAt(strongestIndex);
            StartShake(next);
        }
    }
}
