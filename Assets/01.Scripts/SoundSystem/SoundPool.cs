using System.Collections.Generic;
using UnityEngine;
namespace MINISoundManage
{
    [System.Serializable]

    public class SoundPool
    {
        [SerializeField] private SoundPlayer _soundPlayerPrefab;
        private Stack<SoundPlayer> _pool = new();


        public void Initialize(int defaultPoolAmount)
        {
            for (int i = 0; i < defaultPoolAmount; i++)
            {
                GeneratePoolObject();
            }
        }

        #region External Functions

        public void Clear()
        {

            _pool.Clear();
        }

        public SoundPlayer GetSoundPlayer(Vector3 position)
        {
            SoundPlayer player = _pool.Count > 0
                    ? _pool.Pop()
                    : GenerateSoundPlayer();
            player.gameObject.SetActive(true);
            player.transform.position = position;
            return player;
        }

        #endregion

        private SoundPlayer GenerateSoundPlayer()
        {
            SoundPlayer soundPlayer = Object.Instantiate(_soundPlayerPrefab, Vector3.zero, Quaternion.identity);

            soundPlayer.OnSoundPlayCompleteEvent += HandleSoundPlayOver;
            soundPlayer.gameObject.SetActive(false);
            return soundPlayer;
        }

        private void HandleSoundPlayOver(SoundPlayer player)
        {
            player.ResetItem();
            player.gameObject.SetActive(false);
            _pool.Push(player);
        }

        private void GeneratePoolObject()
        {
            SoundPlayer newPlayer = GenerateSoundPlayer();
            _pool.Push(newPlayer);
        }
    }
}