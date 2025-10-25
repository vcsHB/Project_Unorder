using System.Collections.Generic;
using UnityEngine;

namespace MINISoundManage
{
    public class MusicController : MonoBehaviour
    {
        [Header("BGM Setting")]
        [SerializeField] private float _fadeLength = 1f;
        [SerializeField] private int _musicIndex = -1;
        [SerializeField] private int _playerIndex = 0;
        [SerializeField] private List<SoundSO> _playSequence;
        private AudioSource[] _audioPlayers;

        private AudioSource _currentAudioSource;

        private bool _isFading = false;
        private float _fadeTimer = 0f;
        private float _fadeStartVolume = 0f;
        private float _fadeTargetVolume = 1f;

        private void Awake()
        {
            _audioPlayers = GetComponentsInChildren<AudioSource>();
        }

        private void Start()
        {
            PlayNextAudio();
        }

        private void Update()
        {
            ChangeBGM();
            UpdateFade();
        }

        public void SetMusicSequence(List<SoundSO> list)
        {
            _playSequence = list;
        }

        public void ChangeBGM()
        {
            if (_currentAudioSource == null || _currentAudioSource.clip == null)
                return;

            float remainingTime = _currentAudioSource.clip.length - _currentAudioSource.time;
            if (remainingTime < _fadeLength && !_isFading)
            {
                EndAudio();
                PlayNextAudio();
            }
        }

        public void PlayNextAudio()
        {
            _musicIndex = (_musicIndex + 1) % _playSequence.Count;
            _playerIndex = (_playerIndex + 1) % _audioPlayers.Length;

            _currentAudioSource = _audioPlayers[_playerIndex];
            _currentAudioSource.clip = _playSequence[_musicIndex].clip;
            _currentAudioSource.volume = 0f;
            _currentAudioSource.Play();

            StartFade(_currentAudioSource, 1f);
        }

        public void EndAudio()
        {
            if (_currentAudioSource == null) return;
            StartFade(_currentAudioSource, 0f);
        }

        private void StartFade(AudioSource source, float targetVolume)
        {
            _fadeTimer = 0f;
            _fadeStartVolume = source.volume;
            _fadeTargetVolume = targetVolume;
            _isFading = true;
        }

        private void UpdateFade()
        {
            if (!_isFading || _currentAudioSource == null)
                return;

            _fadeTimer += Time.deltaTime;
            float t = Mathf.Clamp01(_fadeTimer / _fadeLength);
            _currentAudioSource.volume = Mathf.Lerp(_fadeStartVolume, _fadeTargetVolume, t);

            if (t >= 1f)
            {
                _isFading = false;
            }
        }
    }
}
