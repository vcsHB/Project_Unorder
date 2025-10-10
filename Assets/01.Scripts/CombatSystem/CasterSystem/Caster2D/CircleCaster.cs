using UnityEngine;
namespace Project_Unorder.CombatSystem.CasterSystem.Caster2Ds
{

    public class CircleCaster : Caster2D
    {
        [SerializeField] private float _detectRadius = 1f;
        public override void Cast()
        {
            Collider2D[] targets = Physics2D.OverlapCircleAll(CastPivot, _detectRadius, _detectTargetLayer);
            if (targets == null) return;


            ForceCast(targets);
        }



#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (_useDebugVisual)
            {

                Gizmos.color = _gizmosColor;
                Gizmos.DrawWireSphere(CastPivot, _detectRadius);
            }
        }
#endif

    }
}