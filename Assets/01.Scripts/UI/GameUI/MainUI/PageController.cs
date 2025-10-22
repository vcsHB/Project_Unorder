using System;
using UnityEngine;
namespace Project_Unorder.UIManage.InGameSceneUI.MainUIs
{

    public class PageController : MonoBehaviour
    {
        [SerializeField] private WindowPage[] _pages;
        [SerializeField] private WindowPage _defaultPage;
        public WindowPage CurrentWindowPage;
        private bool _isPanelTransition;
        private float _endTime;
        private void Awake()
        {
            for (int i = 0; i < _pages.Length; i++)
            {
                _pages[i].OnPageMoveEvent += HandleMovePage;
            }
        }

        private void Update()
        {
            if (_isPanelTransition)
            {
                if (Time.time > _endTime)
                {
                    _isPanelTransition = false;
                    CurrentWindowPage.HandlePageEnter();
                }
            }
        }

        public void SetDefaultPage()
        {
            HandleMovePage(_defaultPage, 0f);
        }

        private void HandleMovePage(WindowPage nextPage, float transitionDuration)
        {
            if (_isPanelTransition) return;
            if (CurrentWindowPage != null)
                CurrentWindowPage.HandlePageExit();

            CurrentWindowPage = nextPage;
            _endTime = Time.time + transitionDuration;
            _isPanelTransition = true;

        }

        public void HandleMoveControl(Vector2 inputDirection)
        {
            if (_isPanelTransition) return;
            if (CurrentWindowPage == null) return;
            CurrentWindowPage.ReceiveInputDirection(inputDirection);
        }

        public void HandleSubmitControl()
        {
            if (_isPanelTransition) return;
            if (CurrentWindowPage == null) return;
            CurrentWindowPage.ReceiveSubmit();
        }

        public void HandleCancelControl()
        {
            if (_isPanelTransition) return;
            if (CurrentWindowPage == null) return;
            CurrentWindowPage.ReceiveCancel();
        }
    }
}