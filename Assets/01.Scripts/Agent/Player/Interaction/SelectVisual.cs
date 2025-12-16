using UnityEngine;
namespace Project_Unorder.AgentSystem.InteractSystem
{

    public class SelectVisual : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        private readonly int _selectParamHash = Animator.StringToHash("Select");
        private void Awake()
        {
            Debug.Assert(_animator);
        }
        public void Select()
        {
            _animator.SetBool(_selectParamHash, true);
        }

        public void Release()
        {
            _animator.SetBool(_selectParamHash, false);

        }
    }
}