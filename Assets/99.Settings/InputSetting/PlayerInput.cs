using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Project_Unorder.InputManage
{

    [CreateAssetMenu(menuName = "SO/Input/PlayerInput")]
    public class PlayerInput : ScriptableObject, Controls.IPlayerActions
    {
        public Vector2 InputDirection { get; private set; }
        public Vector2 MousePosition { get; private set; }
        public event Action OnMouseClickPressEvent;
        public event Action OnMouseClickReleaseEvent;
        public event Action OnAttackEvent;

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
            if (!GlobalInputConfig.PLAYER_MOVE) return;

            InputDirection = context.ReadValue<Vector2>();
            if (context.canceled)
            {
                InputDirection = Vector2.zero;
            }
        }



        public void OnInteract(InputAction.CallbackContext context)
        {
            if (!GlobalInputConfig.PLAYER_INTERACT) return;

            if (context.performed)
            {
                //TODO
            }
        }

        public void OnDash(InputAction.CallbackContext context)
        {
            if (!GlobalInputConfig.PLAYER_DASH) return;

            if (context.performed)
            {
                // TODO
            }
        }

        public void OnCursorMove(InputAction.CallbackContext context)
        {
            MousePosition = context.ReadValue<Vector2>();
        }

        public void OnCursorClick(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnMouseClickPressEvent?.Invoke();
            }
            else if (context.canceled)
            {
                OnMouseClickReleaseEvent?.Invoke();
            }
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.performed)
                OnAttackEvent?.Invoke();
        }
    }

}