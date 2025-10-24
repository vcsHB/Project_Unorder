using UnityEngine;
namespace Project_Unorder.UIManage.InGameSceneUI.MainUIs
{

    public class WindowPageSelectionItem : WindowSelectionItem
    {
        [SerializeField] private WindowPage _connectPage;
        
        public override SelectResponse Select()
        {
            return new SelectResponse()
            {
                connectPage = _connectPage
            };

        }
    }
}