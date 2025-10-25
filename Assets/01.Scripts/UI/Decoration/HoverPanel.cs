using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
namespace Project_Unorder.UIManage
{

    public class HoverPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public UnityEvent OnMouseEnterEvent;
        public UnityEvent OnMouseExitEvent;
        [SerializeField] private bool _isHoverEnable;

        public virtual void SetHoverEnable(bool value)
        {
            _isHoverEnable = value;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_isHoverEnable)
                OnMouseEnterEvent?.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_isHoverEnable)
                OnMouseExitEvent?.Invoke();
        }
    }
}