using System;
using System.Collections.Generic;
using InputManage;
using UnityEngine;

namespace Project_Unorder.UIManage.InGameSceneUI.MainUIs
{

    public class UISelector : MonoBehaviour
    {
        [SerializeField] private UIInput _uiInput;
        [SerializeField] private List<MainWindow> _windowList = new();
        public MainWindow CurrentWindow { get; private set; }
        private int _currentWindowIndex;

        private void Awake()
        {
            InitializeInput();

            for (int i = 0; i < _windowList.Count; i++)
            {

            }
        }

        private void InitializeInput()
        {
            _uiInput.OnPanelSwitchEvent += HandlePanelSwitch;
            // Panel Switch 

            _uiInput.OnMoveControlEvent += HandleMoveControl;
            _uiInput.OnCancelEvent += HandleCancelControl;
            _uiInput.OnSubmitEvent += HandleSubmitControl;
        }



        private void HandlePanelSwitch()
        {
            if (_windowList == null || _windowList.Count == 0)
            {
                _currentWindowIndex = 0;
                return;
            }
            if (CurrentWindow != null)
            {
                CurrentWindow.HandleUnselect();
            }
            _currentWindowIndex = (_currentWindowIndex + 1) % _windowList.Count;
            CurrentWindow = _windowList[_currentWindowIndex];
            CurrentWindow.HandleSelect();
        }

        #region Controls

        private void HandleMoveControl(Vector2 inputDirection)
        {
            if (CurrentWindow == null) return;
            if (CurrentWindow.IsDenied) return;

            CurrentWindow.HandleMoveControl(inputDirection);
        }

        private void HandleSubmitControl()
        {
            if (CurrentWindow == null) return;
            if (CurrentWindow.IsDenied) return;

            CurrentWindow.HandleSubmitControl();
        }

        private void HandleCancelControl()
        {
            if (CurrentWindow == null) return;
            if (CurrentWindow.IsDenied) return;

            CurrentWindow.HandleCancelControl();
        }

        #endregion
    }
}