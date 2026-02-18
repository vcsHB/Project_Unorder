using UnityEngine;
using UnityEngine.Events;

namespace Project_Unorder.ObjectManage
{
    public class MapBox : MonoBehaviour
    {
        public UnityEvent OnBoxSizingCompleteEvent;

        [Header("Essential Settings")]
        [SerializeField] private Transform _bottomBar;
        [SerializeField] private Transform _topBar;
        [SerializeField] private Transform _leftBar;
        [SerializeField] private Transform _rightBar;
        [SerializeField] private SpriteRenderer _edgeBoxRenderer;

        [Header("Box Setting")]
        [SerializeField] private Vector2 _boxSize = new Vector2(20f, 20f);
        [SerializeField] private float _boxThickness = 0.25f;

        private Vector2 _previousSize;
        private BoxWall[] _walls;
        private SpriteRenderer[] _barRenderers;
        private float _currentDuration;
        private float _sizingDuration;
        private bool _isSizing;

        private static readonly int _useLimitProperty = Shader.PropertyToID("_UseLimit");

        private void Awake()
        {
            _walls = GetComponentsInChildren<BoxWall>();

            _barRenderers = new SpriteRenderer[4];
            _barRenderers[0] = _bottomBar.GetComponent<SpriteRenderer>();
            _barRenderers[1] = _topBar.GetComponent<SpriteRenderer>();
            _barRenderers[2] = _leftBar.GetComponent<SpriteRenderer>();
            _barRenderers[3] = _rightBar.GetComponent<SpriteRenderer>();
        }

        [ContextMenu("Debug10")]
        private void DebugASd()
        {
            SetBoxSize(new Vector2(10f, 10f), 3f, false);
        }

        [ContextMenu("Debug20")]
        private void DebugAasdasd()
        {
            SetBoxSize(new Vector2(20f, 20f), 3f, false);
        }

        public void SetBoxSize(Vector2 newSize, float duration, bool isForce = false)
        {
            if (!isForce && _isSizing) return;

            _currentDuration = 0f;
            _previousSize = _boxSize;
            _boxSize = newSize;
            _sizingDuration = duration;
            _isSizing = true;
        }

        public void SetCollisionMode(bool value)
        {
            if (_walls == null) return;

            _edgeBoxRenderer.color = value ? Color.white : Color.red;
            for (int i = 0; i < _walls.Length; i++)
            {
                _walls[i].SwitchCollisionMode(value);
            }

            int propertyValue = value ? 1 : 0;
            for (int i = 0; i < _barRenderers.Length; i++)
            {
                if (_barRenderers[i] != null)
                {
                    _barRenderers[i].sharedMaterial.SetInteger(_useLimitProperty, propertyValue);
                }
            }
        }

        private void Update()
        {
            if (_isSizing)
            {
                _currentDuration += Time.deltaTime;

                float ratio = _currentDuration / _sizingDuration;
                Vector2 newSize = Vector2.Lerp(_previousSize, _boxSize, ratio);

                SetBoxSize(newSize, _boxThickness);

                if (_currentDuration >= _sizingDuration)
                {
                    _isSizing = false;
                    OnBoxSizingCompleteEvent?.Invoke();
                }
            }
        }

        private void SetBoxSize(Vector2 newSize, float thickness)
        {
            float x = newSize.x;
            float y = newSize.y;

            Vector3 topBottomScale = new Vector3(x + thickness, thickness, 1f);
            Vector3 leftRightScale = new Vector3(thickness, y + thickness, 1f);

            _topBar.localScale = topBottomScale;
            _bottomBar.localScale = topBottomScale;
            _leftBar.localScale = leftRightScale;
            _rightBar.localScale = leftRightScale;

            float halfX = x * 0.5f;
            float halfY = y * 0.5f;

            _edgeBoxRenderer.size = newSize;

            _topBar.localPosition = new Vector3(0f, halfY, 0f);
            _bottomBar.localPosition = new Vector3(0f, -halfY, 0f);
            _leftBar.localPosition = new Vector3(-halfX, 0f, 0f);
            _rightBar.localPosition = new Vector3(halfX, 0f, 0f);
        }
    }
}