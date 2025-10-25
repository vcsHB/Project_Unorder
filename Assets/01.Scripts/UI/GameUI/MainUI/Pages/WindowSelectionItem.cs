using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
namespace Project_Unorder.UIManage.InGameSceneUI.MainUIs
{
    public struct SelectResponse
    {
        public WindowPage connectPage;

    }
    public class WindowSelectionItem : MonoBehaviour
    {
        [SerializeField] private Image _selectMarkImage;
        [SerializeField] private float _selectionFadeDuration = 0.1f;

        public virtual void HandleEnter()
        {
            _selectMarkImage.DOFade(1f, _selectionFadeDuration);
        }

        public virtual void HandleExit()
        {
            _selectMarkImage.DOFade(0f, _selectionFadeDuration);

        }

        public virtual SelectResponse Select()
        {
            return new SelectResponse()
            {

            };
        }


    }
}