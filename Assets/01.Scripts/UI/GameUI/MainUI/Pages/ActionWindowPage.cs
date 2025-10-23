using Unity.AppUI.UI;
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

        }

        private void Select(uint index)
        {
            SelectResponse response = _selectionItems[index].Select();
            if(response.connectPage != null)
            {
                MoveToPage(response.connectPage);
            }
        }
        public override void ReceiveInputDirection(Vector2 inputDirection)
        {
            base.ReceiveInputDirection(inputDirection);
            uint direction = (uint)Mathf.Sign(inputDirection.x);
            if (Mathf.Approximately(direction, 0f)) return;

            _currentSelectionIndex = (uint)(((int)_currentSelectionIndex + direction + _selectionAmount) % _selectionAmount);

        }
    }
}