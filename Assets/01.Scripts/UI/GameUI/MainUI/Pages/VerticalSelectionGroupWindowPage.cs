using UnityEngine;
namespace Project_Unorder.UIManage.InGameSceneUI.MainUIs
{

    public class VerticalSelectionGroupWindowPage : SelectionGroupWindowPage
    {

       
        public override void ReceiveInputDirection(Vector2 inputDirection)
        {
            base.ReceiveInputDirection(inputDirection);
            uint direction = (uint)Mathf.Sign(inputDirection.y);
            if (Mathf.Approximately(direction, 0f)) return;

            _selectionItems[_currentSelectionIndex].HandleExit();
            _currentSelectionIndex = (uint)(((int)_currentSelectionIndex + direction + _selectionAmount) % _selectionAmount);
            _selectionItems[_currentSelectionIndex].HandleEnter();

        }

    }
}