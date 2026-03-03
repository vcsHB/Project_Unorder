using UnityEngine;

namespace Project_Unorder.ObjectManage.VFX
{
    [System.Serializable]
    public struct FloatMinMaxGroup
    {
        public float minValue;
        public float MaxValue;

        public FloatMinMaxGroup(float min, float max)
        {
            minValue = min;
            MaxValue = max;
        }

        public float GetRandom() => Random.Range(minValue, MaxValue);
    }

    [ExecuteAlways]
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteBurstVFX : MonoBehaviour
    {
        private SpriteRenderer _renderer;

        [Header("Settings")]
        [SerializeField] private bool _loop = false;
        [SerializeField] private FloatMinMaxGroup _lifeTimeRange = new FloatMinMaxGroup(0.5f, 1.0f);

        [Header("Transform")]
        [SerializeField] private FloatMinMaxGroup _sizeRange = new FloatMinMaxGroup(0.8f, 1.2f);
        [SerializeField] private AnimationCurve _sizeOverLifeTime = AnimationCurve.Linear(0, 1, 1, 0);
        [SerializeField] private FloatMinMaxGroup _rotationRange = new FloatMinMaxGroup(0f, 360f);

        [Header("Color")]
        [SerializeField] private Gradient _colorOverLifeTime;

        private float _currentLifeTime;
        private float _currentScaleMultiplier;
        private float _currentRotation;
        private float _elapsed;
        private bool _isPlaying;

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            if (!_isPlaying) return;

            _elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(_elapsed / _currentLifeTime);

            ApplyState(t);

            if (t >= 1f)
            {
                if (_loop) Play();
                else Stop();
            }
        }

        public void Play()
        {
            if (_renderer == null) _renderer = GetComponent<SpriteRenderer>();

            _isPlaying = true;
            _elapsed = 0f;

            _currentLifeTime = _lifeTimeRange.GetRandom();
            _currentScaleMultiplier = _sizeRange.GetRandom();
            _currentRotation = _rotationRange.GetRandom();

            transform.localRotation = Quaternion.Euler(0, 0, _currentRotation);
            ApplyState(0);
        }

        public void Stop()
        {
            _isPlaying = false;
            _elapsed = 0f;
            if (Application.isPlaying && !_loop) gameObject.SetActive(false);
        }

        private void ApplyState(float t)
        {
            if (_renderer == null) return;
            transform.localScale = Vector3.one * (_currentScaleMultiplier * _sizeOverLifeTime.Evaluate(t));
            _renderer.color = _colorOverLifeTime.Evaluate(t);
        }
    }
}