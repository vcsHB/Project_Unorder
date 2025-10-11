using System;
using UnityEngine;
using UnityEngine.Events;

namespace Project_Unorder.ObjectManage
{
    public class MapBox : MonoBehaviour
    {
        public UnityEvent OnBoxSizingCompleteEvent;
        [Header("Essential Settings")]
        [SerializeField] Transform _bottomBar;
        [SerializeField] Transform _topBar;
        [SerializeField] Transform _leftBar;
        [SerializeField] Transform _rightBar;

        [Header("Box Setting")]

        [SerializeField] private Vector2 _boxSize = new Vector2(20f, 20f);
        private Vector2 _previousSize;
        [SerializeField] private float _boxThickness = 0.25f;
        private float _currentDuration;
        private float _sizingDuration;
        private bool _isSizing;

        [ContextMenu("Debug10")]
        private void DebugASd()
        {
            SetBoxSize(new Vector2(10f, 10f), _boxThickness, 3f);
        }

        [ContextMenu("Debug20")]
        private void DebugAasdasd()
        {
            SetBoxSize(new Vector2(20f, 20f), _boxThickness, 3f);
        }


        public void SetBoxSize(Vector2 newSize, float thickness, float duration, bool isForce = false)
        {
            if (!isForce && _isSizing) return;

            _currentDuration = 0f;
            _previousSize = _boxSize;
            _boxSize = newSize;
            _sizingDuration = duration;
            _isSizing = true;
        }

        private void Update()
        {
            if (_isSizing)
            {
                _currentDuration += Time.deltaTime;

                float ratio = _currentDuration / _sizingDuration;
                Vector2 newSize = Vector2.Lerp(_previousSize, _boxSize, ratio);
                SetBoxSize(newSize, _boxThickness);
                if (_currentDuration > _sizingDuration)
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

            _topBar.localPosition = new Vector3(0f, halfY, 0f);
            _bottomBar.localPosition = new Vector3(0f, -halfY, 0f);
            _leftBar.localPosition = new Vector3(-halfX, 0f, 0f);
            _rightBar.localPosition = new Vector3(halfX, 0f, 0f);
        }

    }

}