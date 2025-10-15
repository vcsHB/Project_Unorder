using System.Collections.Generic;
using UnityEngine;
namespace MINISoundManage
{

    public class SoundController : MonoSingleton<SoundController>
    {
        [SerializeField] private int _defaultPoolAmount;
        [SerializeField] private float _minSoundPlayInterval = 0.05f;
        [SerializeField] private SoundPool _pool;
        private Dictionary<int, float> _lastPlayTimes = new Dictionary<int, float>();

        private void Start()
        {
            _pool.Initialize(_defaultPoolAmount);
        }

        private void OnDestroy()
        {

            _pool.Clear();
        }

        /// <summary>
        /// Play Souond. Prevents the same clip from playing within a certain amount of time.
        /// </summary>
        /// <param name="soundSO">SoundData for Playing</param>
        /// <param name="position">Play Position</param>
        /// <returns>Generated SoundPlayer Instance</returns>
        public SoundPlayer PlaySound(SoundSO soundSO, Vector2 position)
        {
            if (soundSO == null || soundSO.clip == null)
            {
                Debug.LogError("PlaySound: soundSO 또는 soundSO.clip이 null입니다.");
                return null;
            }

            // 2. Get HashCode to audioClip
            int clipHashCode = soundSO.clip.GetHashCode();
            float currentTime = Time.time;

            // 3. Check Hashcode
            if (_lastPlayTimes.TryGetValue(clipHashCode, out float lastPlayTime))
            {
                // Check elapsed time since Last Playback Time
                if (currentTime - lastPlayTime < _minSoundPlayInterval)
                {
                    // Debug.Log($"[MINISOUND] PlayCanceled: AudioClip HashCode {clipHashCode} was requested again within {_minSoundPlayInterval} seconds");
                    return null;
                }
            }

            // 4. Play Allowed
            SoundPlayer player = _pool.GetSoundPlayer(position);
            player.PlaySound(soundSO);

            // 5. Refresh LastPlayTime
            _lastPlayTimes[clipHashCode] = currentTime;

            return player;
        }




    }
}