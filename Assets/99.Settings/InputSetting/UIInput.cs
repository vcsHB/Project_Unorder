using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputManage
{
    [CreateAssetMenu(menuName = "SO/Input/UIInput")]
    public class UIInput : ScriptableObject, Controls.IUIActions
    {

        public event Action<Vector2> OnMoveControlEvent;
        public event Action OnSubmitEvent;
        public event Action OnCancelEvent;
        public event Action OnPanelSwitchEvent;

        private Controls _controls;

        private void OnEnable()
        {
            if (_controls == null)
            {
                _controls = new Controls();
                _controls.UI.SetCallbacks(this);
            }
            _controls.UI.Enable();
        }

        private void OnDisable()
        {
            _controls.UI.Disable();
        }

        public void OnSubmit(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnSubmitEvent?.Invoke();
            }
        }

        public void OnCancel(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnCancelEvent?.Invoke();
            }
        }

        public void OnPoint(InputAction.CallbackContext context)
        {
        }

        public void OnClick(InputAction.CallbackContext context)
        {
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnMoveControlEvent?.Invoke(context.ReadValue<Vector2>());
            }
        }

        public void OnSwtich(InputAction.CallbackContext context)
        {
            if(context.performed)
            {
                OnPanelSwitchEvent?.Invoke();
            }
        }
    }

}