using System;
using System.Collections.Generic;
using UnityEngine;

namespace MINISoundManage
{
    public enum PlayMode
    {
        Sequential,  // 리스트를 순서대로 한 번만 재생
        LoopAll,     // 리스트 전체 반복
        LoopCurrent  // 현재 곡만 반복
    }
    [RequireComponent(typeof(AudioSource))]
    public class MusicPlayer : MonoBehaviour
    {

        [Header("BGM Setting")]
        [SerializeField] private float _fadeLength = 1f;
        [SerializeField] private PlayMode _playMode = PlayMode.LoopAll;
        [SerializeField] private List<SoundSO> _playSequence;

        private AudioSource[] _audioPlayers;
        private int _playerIndex = 0;
        private int _musicIndex = -1;

        private AudioSource _currentAudioSource;
        private AudioSource _prevAudioSource;

        private bool _isFading = false;
        private float _fadeTimer = 0f;
        private float _fadeOutStartVol = 0f;
        private float _fadeInStartVol = 0f;

        private readonly Queue<SoundSO> _musicQueue = new Queue<SoundSO>();


        private void Awake()
        {
            _audioPlayers = GetComponentsInChildren<AudioSource>();
        }

        private void Start()
        {
            if (_playSequence != null && _playSequence.Count > 0)
                PlayNextAudio();
        }

        private void Update()
        {
            UpdateMusicLifeTime();
            UpdateFade();
        }

        /// <summary>
        /// 재생 모드를 변경합니다.
        /// </summary>
        public void SetPlayMode(PlayMode mode) => _playMode = mode;

        /// <summary>
        /// 단일 곡만 재생되도록 시퀀스를 교체합니다. (현재 재생 중인 곡은 유지)
        /// </summary>
        public void SetSingleMusic(SoundSO newMusic)
        {
            _playSequence.Clear();
            _playSequence.Add(newMusic);
            _musicIndex = 0;
        }

        /// <summary>재생 시퀀스 전체를 교체합니다. (현재 재생 중인 곡은 유지)</summary>
        public void SetMusicSequence(List<SoundSO> list)
        {
            _playSequence = new List<SoundSO>(list);
            _musicIndex = -1;
        }

        /// <summary>
        /// 재생 음악을 변경합니다.
        /// <br/>
        /// <b>forcePlay = true</b>  → 현재 곡을 즉시 크로스페이드하며 재생
        /// <br/>
        /// <b>forcePlay = false</b> → 현재 곡이 끝난 뒤 재생되도록 큐에 추가
        /// </summary>
        public void ChangeMusic(SoundSO data, bool forcePlay)
        {
            if (forcePlay)
                CrossFadeTo(data);
            else
                _musicQueue.Enqueue(data);
        }

        public void EnqueueMusic(SoundSO data) => _musicQueue.Enqueue(data);

        public void Stop() => FadeOutAndStop();

        private void UpdateMusicLifeTime()
        {
            if (_isFading || _currentAudioSource == null || _currentAudioSource.clip == null)
                return;

            float remaining = _currentAudioSource.clip.length - _currentAudioSource.time;
            if (remaining <= _fadeLength)
                OnCurrentTrackEnding();
        }

        private void OnCurrentTrackEnding()
        {
            if (_musicQueue.Count > 0)
            {
                CrossFadeTo(_musicQueue.Dequeue());
                return;
            }

            switch (_playMode)
            {
                case PlayMode.Sequential:
                    if (_musicIndex + 1 < _playSequence.Count)
                        PlayNextAudio();
                    else
                        FadeOutAndStop();
                    break;

                case PlayMode.LoopAll:
                    PlayNextAudio();
                    break;

                case PlayMode.LoopCurrent:
                    ReplayCurrentAudio();
                    break;
            }
        }

        private void PlayNextAudio()
        {
            if (_playSequence == null || _playSequence.Count == 0) return;
            _musicIndex = (_musicIndex + 1) % _playSequence.Count;
            PlayAudioInternal(_playSequence[_musicIndex]);
        }

        private void ReplayCurrentAudio()
        {
            if (_musicIndex < 0 || _musicIndex >= _playSequence.Count) return;
            PlayAudioInternal(_playSequence[_musicIndex]);
        }

        private void CrossFadeTo(SoundSO data) => PlayAudioInternal(data);

        private void FadeOutAndStop()
        {
            if (_currentAudioSource == null) return;
            _prevAudioSource = _currentAudioSource;
            _currentAudioSource = null;
            BeginCrossFade();
        }

        private void PlayAudioInternal(SoundSO data)
        {
            if (data == null || data.clip == null) return;

            _playerIndex = (_playerIndex + 1) % _audioPlayers.Length;

            _prevAudioSource = _currentAudioSource;

            _currentAudioSource = _audioPlayers[_playerIndex];
            _currentAudioSource.clip = data.clip;
            _currentAudioSource.volume = 0f;
            _currentAudioSource.Play();

            BeginCrossFade();
        }

        private void BeginCrossFade()
        {
            _fadeTimer = 0f;
            _fadeOutStartVol = _prevAudioSource != null ? _prevAudioSource.volume : 0f;
            _fadeInStartVol = _currentAudioSource != null ? _currentAudioSource.volume : 0f;
            _isFading = true;
        }

        private void UpdateFade()
        {
            if (!_isFading) return;

            _fadeTimer += Time.deltaTime;
            float t = Mathf.Clamp01(_fadeTimer / _fadeLength);

            if (_prevAudioSource != null)
                _prevAudioSource.volume = Mathf.Lerp(_fadeOutStartVol, 0f, t);

            if (_currentAudioSource != null)
                _currentAudioSource.volume = Mathf.Lerp(_fadeInStartVol, 1f, t);

            if (t >= 1f)
            {
                _isFading = false;

                if (_prevAudioSource != null)
                {
                    _prevAudioSource.Stop();
                    _prevAudioSource.clip = null;
                    _prevAudioSource = null;
                }
            }
        }
    }
}