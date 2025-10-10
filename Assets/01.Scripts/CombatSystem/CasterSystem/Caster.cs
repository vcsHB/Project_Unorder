using Project_Unorder.Core.Attribute;
using UnityEngine;
using UnityEngine.Events;
namespace Project_Unorder.CombatSystem.CasterSystem
{

    public abstract class Caster : MonoBehaviour
    {
        public UnityEvent OnCastSuccessEvent;
        public UnityEvent OnCastEvent;
        [Header("Debug Settings")]
        [SerializeField] protected bool _useDebugVisual;
        [SerializeField, ShowIf(nameof(_useDebugVisual))] protected Color _gizmosColor = Color.red;
        [Space(5f)]
        [Header("Basic Settings")]
        [SerializeField] protected int _detectMaxTargetAmount = 3;
        [SerializeField] protected LayerMask _detectTargetLayer;
        [SerializeField] protected Vector3 _castOffset;

        public Vector3 CastPivot => transform.position + _castOffset;


        protected virtual void Initialize()
        {

        }

        public abstract void Cast();





        #region Target Layer Control External Functions

        public void SetTargetLayer(LayerMask newLayerMask)
        {
            _detectTargetLayer = newLayerMask;
        }

        public void ApplyTargetLayer(LayerMask newTarget)
        {
            _detectTargetLayer |= newTarget;
        }
        #endregion

    }
}