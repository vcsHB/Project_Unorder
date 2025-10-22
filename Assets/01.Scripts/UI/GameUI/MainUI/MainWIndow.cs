using System;
using Project_Unorder.Core.Attribute;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
namespace Project_Unorder.UIManage.InGameSceneUI.MainUIs
{

    public class MainWindow : MonoBehaviour, IDeniable, IWindowPanel
    {
        private DenyPanel _denyPanel;
        [SerializeField, ReadOnly] private PageController _pageController;
        public bool IsDenied { get; protected set; }


        private void Awake()
        {
            _denyPanel = GetComponentInChildren<DenyPanel>();
            if (_denyPanel == null)
            {
                Debug.LogError("[MainWindow Initialize] DenyPanel is not Attached");
                return;
            }
        }
        public void SetDeny(bool value)
        {
            IsDenied = value;
            if (value)
            {
                _denyPanel.Open();
            }
            else
            {
                _denyPanel.Close();
            }
        }


        public virtual void Open()
        {
        }

        public virtual void Close()
        {
        }


        #region  PageControl

        public virtual void HandleMoveControl(Vector2 inputDirection)
        {
            _pageController.HandleMoveControl(inputDirection);
        }

        public virtual void HandleSubmitControl()
        {
            _pageController.HandleSubmitControl();
        }

        public virtual void HandleCancelControl()
        {
            _pageController.HandleCancelControl();
        }

        #endregion
    }
}