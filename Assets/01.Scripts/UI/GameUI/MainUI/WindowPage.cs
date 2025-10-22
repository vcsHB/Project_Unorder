using System;
using UnityEngine;
namespace Project_Unorder.UIManage.InGameSceneUI.MainUIs
{

    public abstract class WindowPage : MonoBehaviour
    {
        protected IWindowPanel _panel;
        [SerializeField] protected float _transitionExitDuration = 0.3f;
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
            _panel.Open();
        }

        public virtual void HandlePageExit()
        {
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