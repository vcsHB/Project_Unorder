using UnityEngine;
namespace Project_Unorder.UIManage.InGameSceneUI.MainUIs
{
    public struct SelectResponse
    {
        public WindowPage connectPage;
        
    }
    public class WindowSelectionItem : MonoBehaviour
    {
        public virtual void HandleSelect()
        {

        }

        public virtual void HandleUnselect()
        {

        }

        public virtual SelectResponse Select()
        {
            return new SelectResponse()
            {

            };
        } 
        
        
    }
}