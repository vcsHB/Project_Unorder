using UnityEngine;
namespace Project_Unorder.UIManage.InGameSceneUI.MainUIs
{

    public class ActionViewWindow : MainWindow
    {

        public override void HandleSelect()
        {
            if (_pageController.CurrentWindowPage != null)
            {

                _pageController.CurrentWindowPage.HandlePageExit();
                _pageController.CurrentWindowPage = null;
            }
            base.HandleSelect();
        }

    }
}