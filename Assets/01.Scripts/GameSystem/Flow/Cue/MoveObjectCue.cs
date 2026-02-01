using DG.Tweening;
using UnityEngine;
namespace Project_Unorder.FlowSystem
{

    public class MoveObjectCue : FlowCue
    {
        [SerializeField] protected Transform _targetTrm;
        [SerializeField] protected Vector2 _targetPosition;
        [SerializeField] protected float _duration = 1f;
        [SerializeField] protected Ease _ease;

#if UNITY_EDITOR
        [Header("Gizmos Setting")]
        [SerializeField] protected Color _gizmosColor = Color.green;
        [SerializeField] protected Color _targetPositionGizmosColor = Color.green;
        [SerializeField] protected float _gizmosScale = 0.3f;
#endif

        public override void Execute()
        {
            _targetTrm.DOMove(_targetPosition, _duration)
                .SetEase(_ease)
                .OnComplete(InvokeCueComplete);
        }

#if UNITY_EDITOR
        protected void OnDrawGizmos()
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