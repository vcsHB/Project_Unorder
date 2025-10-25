using System;
using DG.Tweening;
using UnityEngine;
namespace Project_Unorder.UIManage.InGameSceneUI
{

    public class HoverScaleHighlighter : HoverPanel
    {
        [Header("Highlight Setting")]
        [SerializeField] private Vector3 _highlightEnableScale = new Vector3(1.1f, 1.1f, 1.1f);
        [SerializeField] private Vector3 _highlightDisableScale = Vector3.one;
        [SerializeField] private float _duration = 0.3f;
        
        private void Awake()
        {

            OnMouseEnterEvent.AddListener(HandleMouseEnter);
            OnMouseExitEvent.AddListener(HandleMouseExit);
        }

        private void HandleMouseEnter()
        {
            transform.DOScale(_highlightEnableScale, _duration);
        }

        private void HandleMouseExit()
        {
            transform.DOScale(_highlightDisableScale, _duration);
        }
    }
}