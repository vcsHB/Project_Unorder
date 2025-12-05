using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputManage
{
    [CreateAssetMenu(menuName = "SO/Input/PlayerInput")]
    public class PlayerInput : ScriptableObject, Controls.IPlayerActions
    {
        public Vector2 InputDirection { get; private set; }
        public Vector2 MousePosition { get; private set; }

        private Controls _controls;

        private void OnEnable()
        {
            if (_controls == null)
            {
                _controls = new Controls();
                _controls.Player.SetCallbacks(this);
            }
            _controls.Player.Enable();
        }

        private void OnDisable()
        {
            _controls.Player.Disable();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            InputDirection = context.ReadValue<Vector2>();
            if (context.canceled)
            {
                InputDirection = Vector2.zero;
            }
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.performed)
            {

            }
        }

        public void OnDash(InputAction.CallbackContext context)
        {
            if (context.performed)
            {

            }
        }

        public void OnMouseDelta(InputAction.CallbackContext context)
        {
            
            InputDirection = context.ReadValue<Vector2>();
             
        }
    }

}