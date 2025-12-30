using UnityEngine;
namespace Project_Unorder.UIManage.InGameSceneUI.MainUIs
{

    public class SelectionGroupWindowPage : WindowPage
    {
        [SerializeField] protected WindowSelectionItem[] _selectionItems;

        protected uint _currentSelectionIndex;
        protected uint _selectionAmount;
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

        protected void Select(uint index)
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

    }
}