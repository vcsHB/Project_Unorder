using System;
using UnityEngine;
using UnityEngine.Audio;

namespace MINISoundManage
{

    public class SoundPlayer : MonoBehaviour
    {
        [SerializeField] private AudioMixerGroup _sfxGroup, _musicGroup;
        public event Action<SoundPlayer> OnSoundPlayCompleteEvent;

        private AudioSource _audioSource;
        private bool _isPlaying;
        private float _endTime;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }


        public void PlaySound(SoundSO data)
        {
            if (data.audioType == AudioType.SFX)
                _audioSource.outputAudioMixerGroup = _sfxGroup;
            else if (data.audioType == AudioType.BGM)
                _audioSource.outputAudioMixerGroup = _musicGroup;

            _audioSource.volume = data.volume;
            _audioSource.pitch = data.pitch;
            if (data.randomizePitch)
                _audioSource.pitch += UnityEngine.Random.Range(-data.randomPitchModifier, data.randomPitchModifier);

            _audioSource.Stop();
            _audioSource.clip = data.clip;
            _audioSource.loop = data.loop;
            _audioSource.Play();
            _isPlaying = true;
            _endTime = Time.time + _audioSource.clip.length / _audioSource.pitch;

        }

        private void Update()
        {
            if (_isPlaying && Time.time >= _endTime)
            {
                _isPlaying = false;
                OnSoundPlayCompleteEvent?.Invoke(this);
            }
        }

        public void SetForceOverSound()
        {
            _audioSource.Stop();
            _isPlaying = false;
            OnSoundPlayCompleteEvent?.Invoke(this);
        }

        public void ResetItem()
        {
            _audioSource.Stop();
            _audioSource.clip = null;

        }
    }

}