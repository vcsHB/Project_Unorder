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


        public SoundPlayer PlaySound(SoundSO soundSO, Vector2 position)
        {
            if (soundSO == null || soundSO.clip == null)
            {
                Debug.LogError("PlaySound: soundSO 또는 soundSO.clip이 null입니다.");
                return null;
            }

            // 2. 오디오 클립의 해시 코드 가져오기
            int clipHashCode = soundSO.clip.GetHashCode();
            float currentTime = Time.time;

            // 3. 해시코드를 체크하여 일정 시간 내 재생 여부 확인
            if (_lastPlayTimes.TryGetValue(clipHashCode, out float lastPlayTime))
            {
                // 마지막 재생 시간으로부터 경과된 시간 체크
                if (currentTime - lastPlayTime < _minSoundPlayInterval)
                {
                    // 일정 시간 내에 재생되었으므로 재생 취소
                    // Debug.Log($"재생 취소: 클립 해시코드 {clipHashCode}가 {_minSoundPlayInterval}초 이내에 다시 요청됨.");
                    return null;
                }
            }

            // 4. 재생이 허용되면, 사운드 풀에서 SoundPlayer를 가져와 재생
            SoundPlayer player = _pool.GetSoundPlayer(position);
            player.PlaySound(soundSO);

            // 5. 마지막 재생 시간을 갱신
            _lastPlayTimes[clipHashCode] = currentTime;

            return player;
        }




    }
}