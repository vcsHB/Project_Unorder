using TMPro;
using UnityEngine;
namespace Project_Unorder.ObjectManage
{

    public class AlphaGroupController : MonoBehaviour
    {
        private SpriteRenderer[] _spriteRenderers;
        private TextMeshPro[] _textMeshes;
        [SerializeField, Range(0f, 1f)] private float _groupAlpha = 1f;
        private float _currentTime;
        private float _duration;
        private float _previousAlpha;
        private float _targetAlpha;
        private bool _isTweening;



        private void Awake()
        {
            _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
            _textMeshes = GetComponentsInChildren<TextMeshPro>();

            UpdateAlpha(); // Init Alpha Set
        }


        #region External Functions


        /// <summary>
        /// FadeIn to Alpha 1.
        /// </summary>
        /// <param name="duration">Fade Effect Play Duration</param>
        /// <param name="isForce">Ignore Fade Effect in progress</param>
        public void Open(float duration, bool isForce = false)
        {
            if (!isForce && _isTweening) return;
            SetAlpha(1f, duration, isForce);

        }
        /// <summary>
        /// FadeOut to Alpha 0.
        /// </summary>
        /// <param name="duration">Fade Effect Play Duration</param>
        /// <param name="isForce">Ignore Fade Effect in progress</param>
        public void Close(float duration, bool isForce = false)
        {
            if (!isForce && _isTweening) return;
            SetAlpha(0f, duration, isForce);

        }

        public void SetAlpha(float newAlpha, float duration, bool isForce = false)
        {
            if (!isForce && _isTweening) return;

            _previousAlpha = _groupAlpha;
            _targetAlpha = newAlpha;
            _isTweening = true;
            _currentTime = 0f;
            _duration = duration;

        }

        #endregion

        private void Update()
        {
            if (_isTweening)
            {
                _currentTime += Time.deltaTime;
                float time = _currentTime / _duration;
                float newAlpha = Mathf.Lerp(_previousAlpha, _targetAlpha, time);
                SetAlpha(newAlpha);
                if (_currentTime > _duration)
                {
                    _isTweening = false;
                    SetAlpha(_targetAlpha);
                }
            }
        }

        private void SetAlpha(float newAlpha)
        {
            _groupAlpha = newAlpha;
            UpdateAlpha();
        }

        private void UpdateAlpha()
        {
            Color color;
            foreach (SpriteRenderer spriteRenderer in _spriteRenderers)
            {
                color = spriteRenderer.color;
                color.a = _groupAlpha;
                spriteRenderer.color = color;
            }

            foreach (TextMeshPro text in _textMeshes)
            {
                color = text.color;
                color.a = _groupAlpha;
                text.color = color;
            }
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            if (_spriteRenderers == null || _spriteRenderers.Length == 0)
                _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
            if (_textMeshes == null || _textMeshes.Length == 0)
                _textMeshes = GetComponentsInChildren<TextMeshPro>();

            SetAlpha(_groupAlpha);


        }
#endif

    }
}