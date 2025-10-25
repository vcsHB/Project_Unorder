using UnityEngine;
namespace Project_Unorder.AgentSystem
{
    [RequireComponent(typeof(Animator))]
    public class AgentRenderer : MonoBehaviour
    {
        protected SpriteRenderer _spriteRenderer;
        public Animator Animator { get; private set; }

        protected virtual void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            Animator = GetComponent<Animator>();
            if (Animator == null)
            {
                Debug.LogError("[AgentSystem:AgentRenderer] Animator is null");
                return;
            }

        }



        public void SetParam(AnimParamSO param, bool value) => Animator.SetBool(param.animHash, value);
        public void SetParam(AnimParamSO param, float value) => Animator.SetFloat(param.animHash, value);
        public void SetParam(AnimParamSO param, int value) => Animator.SetInteger(param.animHash, value);
        public void SetParam(AnimParamSO param) => Animator.SetTrigger(param.animHash);



    }
}