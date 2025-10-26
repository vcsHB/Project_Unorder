using UnityEngine;
using System;

namespace Project_Unorder.UIManage.InGameSceneUI.MainUIs
{

    [Serializable]
    public class SelectionRow
    {
        public WindowSelectionItem[] items;
    }

    
    public class GridSelectionGroupWindowPage : WindowPage
    {
        [SerializeField] protected SelectionRow[] _selectionRows;

        protected Vector2Int _currentSelectionPosition;

        private uint _gridWidth;
        private uint[] _gridHeights;

        protected override void Awake()
        {
            base.Awake();

            _gridWidth = (uint)_selectionRows.Length;

            _gridHeights = new uint[_gridWidth];
            for (int i = 0; i < _gridWidth; i++)
            {
                if (_selectionRows[i] != null && _selectionRows[i].items != null)
                {
                    _gridHeights[i] = (uint)_selectionRows[i].items.Length;
                }
                else
                {
                    _gridHeights[i] = 0;
                }
            }
            _currentSelectionPosition = Vector2Int.zero;
        }

        public override void HandlePageEnter()
        {
            base.HandlePageEnter();

            _currentSelectionPosition = Vector2Int.zero;

            if (_gridWidth > 0 && _gridHeights[0] > 0)
            {
                _selectionRows[_currentSelectionPosition.x].items[_currentSelectionPosition.y].HandleEnter();
            }
        }

        protected void Select(Vector2Int position)
        {
            int x = position.x;
            int y = position.y;

            if (x >= 0 && x < _gridWidth && y >= 0 && y < _gridHeights[x])
            {
                SelectResponse response = _selectionRows[x].items[y].Select();
                if (response.connectPage != null)
                {
                    MoveToPage(response.connectPage);
                }
            }
        }

        public override void ReceiveSubmit()
        {
            base.ReceiveSubmit();
            Select(_currentSelectionPosition);
        }

        public override void ReceiveInputDirection(Vector2 inputDirection)
        {
            base.ReceiveInputDirection(inputDirection);

            int currentX = _currentSelectionPosition.x;
            int currentY = _currentSelectionPosition.y;

            int deltaX = Mathf.RoundToInt(inputDirection.x);
            int deltaY = Mathf.RoundToInt(inputDirection.y);

            if (deltaX == 0 && deltaY == 0) return;

            if (currentX >= 0 && currentX < _gridWidth &&
                currentY >= 0 && currentY < _gridHeights[currentX])
            {
                _selectionRows[currentX].items[currentY].HandleExit();
            }
            else
            {
                return;
            }

            if (deltaX != 0)
            {
                int newX = (currentX + deltaX);

                if (newX < 0)
                {
                    newX = (int)_gridWidth - 1;
                }
                else if (newX >= _gridWidth)
                {
                    newX = 0;
                }

                if (_gridHeights[newX] > 0)
                {
                    currentX = newX;
                    currentY = Mathf.Min(currentY, (int)_gridHeights[currentX] - 1);
                }
            }

            if (deltaY != 0)
            {
                if (_gridHeights[currentX] > 0)
                {
                    int newY = (currentY + deltaY);
                    int columnHeight = (int)_gridHeights[currentX];

                    if (newY < 0)
                    {
                        newY = columnHeight - 1;
                    }
                    else if (newY >= columnHeight)
                    {
                        newY = 0;
                    }

                    currentY = newY;
                }
            }

            _currentSelectionPosition = new Vector2Int(currentX, currentY);

            if (currentX >= 0 && currentX < _gridWidth &&
                currentY >= 0 && currentY < _gridHeights[currentX])
            {
                _selectionRows[_currentSelectionPosition.x].items[_currentSelectionPosition.y].HandleEnter();
            }
        }
    }
}