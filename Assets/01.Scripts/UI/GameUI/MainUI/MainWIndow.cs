using DG.Tweening;
using Project_Unorder.Core.Attribute;
using UnityEngine;
using UnityEngine.UI;
namespace Project_Unorder.UIManage.InGameSceneUI.MainUIs
{

    public class MainWindow : MonoBehaviour, IDeniable, IWindowPanel
    {
        private DenyPanel _denyPanel;
        [SerializeField] private Image _backgroundImage;
        [SerializeField, ReadOnly] private PageController _pageController;
        [Header("Select Setting")]
        [SerializeField] private float _selectScale = 1.03f;
        [SerializeField] private Color _selectColor = Color.white;
        [SerializeField] private Color _unselectColor = new Color(0.8f, 0.8f, 0.8f);
        [SerializeField] private float _selectTweenDuration = 0.1f;
        public bool IsDenied { get; protected set; }


        private void Awake()
        {
            _denyPanel = GetComponentInChildren<DenyPanel>();
            if (_denyPanel == null)
            {
                Debug.LogError("[MainWindow Initialize] DenyPanel is not Attached");
                return;
            }
            _pageController = GetComponent<PageController>();
            if (_pageController == null)
            {
                Debug.LogError("[MainWindow Initialize] PageController is not Attached");
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


        #region  External Functions

        public virtual void HandleSelect()
        {
            transform.DOScale(_selectScale, _selectTweenDuration);
            _pageController.SetDefaultPage();
            _backgroundImage.DOColor(_selectColor, _selectTweenDuration);
        }

        public virtual void HandleUnselect()
        {
            transform.DOScale(1f, _selectTweenDuration);
            _backgroundImage.DOColor(_unselectColor, _selectTweenDuration);

        }

        public virtual void Open()
        {
        }

        public virtual void Close()
        {
        }

        #endregion


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