using UnityEngine;
namespace Project_Unorder.UIManage.InGameSceneUI.MainUIs
{

    public class ObserverWindowPage : WindowPage
    {
        [SerializeField] private WindowPage _nextPage;

        public override void ReceiveSubmit()
        {
            base.ReceiveSubmit();
            MoveToPage(_nextPage);

        }
    }


}