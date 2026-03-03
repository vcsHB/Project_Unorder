using ObjectPooling;
using UnityEngine;
namespace Project_Unorder.ObjectManage.VFX
{

    public class VFXPlayer : MonoBehaviour, IPoolable
    {
        [SerializeField] private SpriteBurstVFX[] _spriteVFXs;
        [SerializeField] private ParticleSystem _particleVFX;
        [SerializeField] private float _lifeTime = 5f;

        public GameObject GameObject => gameObject;

        public void OnPop()
        {
            Play();
        }

        public void OnPush()
        {

        }

        public void Play()
        {
            if (_spriteVFXs.Length > 0)
                for (int i = 0; i < _spriteVFXs.Length; i++)
                {
                    _spriteVFXs[i].Play();
                }
            _particleVFX.Play();
            Invoke(nameof(ReturnToPool), _lifeTime);
        }

        private void ReturnToPool()
        {
            ObjectPool.Push(this);
        }

        public void Stop()
        {
            if (_spriteVFXs.Length > 0)
                for (int i = 0; i < _spriteVFXs.Length; i++)
                {
                    _spriteVFXs[i].Stop();
                }
            _particleVFX.Stop();
        }

    }
}