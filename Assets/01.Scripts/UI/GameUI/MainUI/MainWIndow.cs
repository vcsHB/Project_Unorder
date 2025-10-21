using UnityEngine;
namespace Project_Unorder.UIManage.InGameSceneUI.MainUIs
{

    public class MainWindow : MonoBehaviour, IDeniable, IWindowPanel
    {
        private DenyPanel _denyPanel;


        private void Awake()
        {
            _denyPanel = GetComponentInChildren<DenyPanel>();
            if (_denyPanel == null)
            {
                Debug.LogError("[MainWindow Initialize] DenyPanel is not Attached");
                return;
            }
        }
        public void SetDeny(bool value)
        {
            if (value)
            {
                _denyPanel.Open();
            }
            else
            {
                _denyPanel.Close();
            }
        }

        public virtual void Open()
        {
        }

        public virtual void Close()
        {
        }
    }
}