using DG.Tweening;
using UnityEngine;
namespace Project_Unorder.FlowSystem
{

    public class MoveObjectCue : FlowCue
    {
        [SerializeField] private Transform _targetTrm;
        [SerializeField] private Vector2 _targetPosition;
        [SerializeField] private float _duration = 1f;
        [SerializeField] private Ease _ease;

#if UNITY_EDITOR
        [Header("Gizmos Setting")]
        [SerializeField] private Color _gizmosColor = Color.green;
        [SerializeField] private Color _targetPositionGizmosColor = Color.green;
        [SerializeField] private float _gizmosScale = 0.3f;
#endif

        public override bool Execute()
        {
            _targetTrm.DOMove(_targetPosition, _duration).SetEase(_ease);
            return true;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (_targetTrm == null)
                return;
            Gizmos.color = _gizmosColor;
            Gizmos.DrawSphere(_targetPosition, _gizmosScale);
            Gizmos.DrawLine(_targetTrm.position, _targetPosition);
            Gizmos.color = _targetPositionGizmosColor;
            Gizmos.DrawSphere(_targetTrm.position, _gizmosScale);
        }
#endif
    }
}