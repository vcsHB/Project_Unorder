using System.Collections;
using ObjectPooling;
using UnityEngine;
namespace Project_Unorder.ObjectManage.VFX
{

    public class SliceVFX : MonoBehaviour, IPoolable
    {
        [SerializeField] private TrailRenderer _trailRenderer;
        private Transform _trailTrm;
        [SerializeField] private ParticleSystem _burstParticles;
        [SerializeField] private SpriteBurstVFX _spriteVFX;
        [SerializeField] private ParticleSystem _directionVFX;
        [SerializeField] private float _slashDuration = 0.3f;
        [SerializeField] private float _slashScale = 5f;
        [SerializeField] private float _lifeTime = 4f;

        public GameObject GameObject => gameObject;


        private void Awake()
        {
            Debug.Assert(_trailRenderer);
            _trailTrm = _trailRenderer.transform;
            Debug.Assert(_slashDuration != 0);
        }
        public void OnPop()
        {
        }

        public void OnPush()
        {
        }

        public void Slash(Vector2 pivot, Vector2 direction)
        {
            transform.position = pivot;
            direction.Normalize();
            Vector2 slashDirection = direction * _slashScale;
            Vector2 startPosition = pivot + -slashDirection;
            Vector2 endPosition = pivot + slashDirection;

            // VFX
            _spriteVFX.Play();
            _burstParticles.Play();
            _directionVFX.transform.right = direction;
            _directionVFX.Play();

            StartCoroutine(SlashRoutine(startPosition, endPosition));
        }

        private IEnumerator SlashRoutine(Vector2 start, Vector2 end)
        {
            float currentTime = 0f;
            while (currentTime < _slashDuration)
            {
                currentTime += Time.deltaTime;
                float ratio = currentTime / _slashDuration;
                _trailTrm.position = Vector2.Lerp(start, end, ratio);

                yield return null;
            }
            _trailTrm.position = end;

        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _slashScale);
        }
    }
}