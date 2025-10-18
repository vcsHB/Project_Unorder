using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace Project_Unorder.ObjectManage.CombatObjects.ProcessWindows
{
    public class ProcessWindow : MonoBehaviour
    {
        [SerializeField] private Vector2 _size = new Vector2(10f, 4.5f);
        [SerializeField] private AlphaGroupController _contentGroup;
        [SerializeField] private SpriteRenderer _windowRenderer;
        private const float _disableHeight = 0.1f;
        [SerializeField] private float _enableDuration = 0.2f;
        [SerializeField] private float _moveSpeed = 2f;
        [SerializeField] private bool _isWindowEnable;
        public bool IsWindowEnable => _isWindowEnable;

        private Coroutine _currentSizeCoroutine;
        private Vector2 _targetSize;

        private void Awake()
        {
            if (_windowRenderer != null)
            {
                _windowRenderer.size = new Vector2(_size.x, _disableHeight);
                if (_contentGroup != null)
                {
                    _contentGroup.SetAlpha(0f, 0f, true);
                }
            }
        }

        #region External Functions

        [ContextMenu("DebugOpen")]
        public void Open()
        {
            if (_currentSizeCoroutine != null) StopCoroutine(_currentSizeCoroutine);
            gameObject.SetActive(true);

            _currentSizeCoroutine = StartCoroutine(OpenCoroutine());
        }
        [ContextMenu("DebugClose")]
        public void Close()
        {
            if (_currentSizeCoroutine != null) StopCoroutine(_currentSizeCoroutine);
            
            _currentSizeCoroutine = StartCoroutine(CloseCoroutine());
        }

        public void Move(Vector2 position, Ease ease = Ease.Linear)
        {
            transform.DOKill(true);
            
            float distance = (position - (Vector2)transform.position).magnitude;
            float duration = distance / _moveSpeed; 
            
            transform.DOMove(position, duration).SetEase(ease);
        }

        #endregion

        #region Coroutines

        private IEnumerator OpenCoroutine()
        {
            Vector2 startSizeX = _windowRenderer.size;
            _targetSize = new Vector2(_size.x, _disableHeight);
            yield return StartCoroutine(ResizeCoroutine(startSizeX, _targetSize));

            Vector2 startSizeY = _windowRenderer.size;
            _targetSize = _size;
            yield return StartCoroutine(ResizeCoroutine(startSizeY, _targetSize));

            if (_contentGroup != null)
            {
                _contentGroup.Open(_enableDuration, true);
                yield return new WaitForSeconds(_enableDuration);
            }
        }

        private IEnumerator CloseCoroutine()
        {
            if (_contentGroup != null)
            {
                _contentGroup.Close(_enableDuration, true);
                yield return new WaitForSeconds(_enableDuration);
            }

            Vector2 startSizeY = _windowRenderer.size;
            _targetSize = new Vector2(_size.x, _disableHeight);
            yield return StartCoroutine(ResizeCoroutine(startSizeY, _targetSize));

            Vector2 startSizeX = _windowRenderer.size;
            _targetSize = new Vector2(_disableHeight, _disableHeight);
            yield return StartCoroutine(ResizeCoroutine(startSizeX, _targetSize));

            //gameObject.SetActive(false);
        }

        private IEnumerator ResizeCoroutine(Vector2 startSize, Vector2 endSize)
        {
            float timer = 0f;
            while (timer < _enableDuration)
            {
                timer += Time.deltaTime;
                float ratio = Mathf.Clamp01(timer / _enableDuration);
                
                float easedRatio = Mathf.Sin(ratio * Mathf.PI * 0.5f); // OutSine
                
                _windowRenderer.size = Vector2.Lerp(startSize, endSize, easedRatio);
                yield return null;
            }
            _windowRenderer.size = endSize;
        }

        #endregion

        #if UNITY_EDITOR
        private void OnValidate()
        {
            if (_windowRenderer == null) return;
            if (!Application.isPlaying)
            {
                _windowRenderer.size = _size;
            }
        }
        #endif
    }
}