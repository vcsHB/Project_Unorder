using System;
using UnityEngine;
namespace Project_Unorder.UIManage.InGameSceneUI.MainUIs
{

    public abstract class WindowPage : MonoBehaviour
    {
        protected IWindowPanel _panel;
        [SerializeField] protected float _transitionExitDuration = 0.3f;
        public float TransitionExitDuration => _transitionExitDuration;
        public event Action<WindowPage, float> OnPageMoveEvent; // NextPage / Transition Duration

        protected virtual void Awake()
        {
            _panel = GetComponent<IWindowPanel>();
        }

        protected virtual void MoveToPage(WindowPage nextPage)
        {
            OnPageMoveEvent?.Invoke(nextPage, _transitionExitDuration);
        }

        public virtual void HandlePageEnter()
        {
            if (_panel != null)
                _panel.Open();
        }

        public virtual void HandlePageExit()
        {
            if (_panel != null)
                _panel.Close();
        }

        public virtual void ReceiveInputDirection(Vector2 inputDirection)
        {

        }

        public virtual void ReceiveSubmit()
        {

        }

        public virtual void ReceiveCancel()
        {

        }


    }
}