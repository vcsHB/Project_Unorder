using UnityEngine;

namespace Project_Unorder.AgentSystem.InteractSystem
{
    public class DragZone : MonoBehaviour
    {
        [SerializeField] private DragZoneVisual _visual;
        [SerializeField] private LayerMask _detectTargetLayer;
        [SerializeField] private uint _maxDragTargetAmount = 5;
        private Vector2 _center;
        private Vector2 _size;
        private bool _isDragging;
        private Vector2 _dragStartPos;
        private Vector2 _currentPos;

        public void HandleDragStart(Vector2 position)
        {
            _isDragging = true;
            _dragStartPos = position;
            _currentPos = position;

            _visual.transform.position = position; // 기준점 고정
            _visual.SetActive(true);
            _visual.UpdateSize(_dragStartPos, _currentPos);
        }

        public void SetMousePosition(Vector2 position)
        {
            if (!_isDragging) return;

            _currentPos = position;
            _visual.UpdateSize(_dragStartPos, _currentPos);
        }

        public void Release()
        {
            if (!_isDragging) return;
            _isDragging = false;

            _visual.SetActive(false);

            _center = (_dragStartPos + _currentPos) * 0.5f;
            _size = new Vector2(
                Mathf.Abs(_dragStartPos.x - _currentPos.x),
                Mathf.Abs(_dragStartPos.y - _currentPos.y)
            );


            Collider2D[] hits = Physics2D.OverlapBoxAll(
                _center,
                _size,
                0f,
                _detectTargetLayer
            );

            uint count = 0;
            foreach (var hit in hits)
            {
                if (count >= _maxDragTargetAmount) break;

                if (hit.TryGetComponent(out ISelectable selectable))
                {
                    selectable.Select();
                    count++;
                }
            }
        }
        void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(_center, _size);

        }
    }
}
