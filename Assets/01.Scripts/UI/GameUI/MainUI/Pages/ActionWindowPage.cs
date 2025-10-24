using UnityEngine;
namespace Project_Unorder.UIManage.InGameSceneUI.MainUIs
{

    public class ActionWindowPage : WindowPage
    {
        [SerializeField] private WindowSelectionItem[] _selectionItems;
        private uint _currentSelectionIndex;
        private uint _selectionAmount;
        protected override void Awake()
        {
            base.Awake();
            _selectionAmount = (uint)_selectionItems.Length;

        }

        public override void HandlePageEnter()
        {
            base.HandlePageEnter();
            for (short i = 0; i < _selectionAmount; i++)
            {
                _selectionItems[i].HandleExit();
            }
            _currentSelectionIndex = 0;
            _selectionItems[_currentSelectionIndex].HandleEnter();
        }

        private void Select(uint index)
        {
            SelectResponse response = _selectionItems[index].Select();
            if (response.connectPage != null)
            {
                MoveToPage(response.connectPage);
            }
        }

        public override void ReceiveSubmit()
        {
            base.ReceiveSubmit();
            Select(_currentSelectionIndex);
        }
        public override void ReceiveInputDirection(Vector2 inputDirection)
        {
            base.ReceiveInputDirection(inputDirection);
            uint direction = (uint)Mathf.Sign(inputDirection.x);
            if (Mathf.Approximately(direction, 0f)) return;

            _selectionItems[_currentSelectionIndex].HandleExit();
            _currentSelectionIndex = (uint)(((int)_currentSelectionIndex + direction + _selectionAmount) % _selectionAmount);
            _selectionItems[_currentSelectionIndex].HandleEnter();



        }
    }
}