using UnityEngine;
namespace Project_Unorder.ObjectManage
{

    public class VFXPlayer : MonoBehaviour
    {
        [SerializeField] private SpriteBurstVFX[] _spriteVFXs;
        [SerializeField] private ParticleSystem _particleVFX;

        public void Play()
        {
            if (_spriteVFXs.Length > 0)
                for (int i = 0; i < _spriteVFXs.Length; i++)
                {
                    _spriteVFXs[i].Play();
                }
            _particleVFX.Play();
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