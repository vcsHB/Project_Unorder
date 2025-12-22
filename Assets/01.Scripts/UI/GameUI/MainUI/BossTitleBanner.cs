using System;
using UnityEngine;

namespace Project_Unorder.UIManage.InGameSceneUI.MainUIs
{

    public class BossTitleBanner : MonoBehaviour
    {
        [SerializeField] private Animation _showAnimation;
        [SerializeField] private string _animationName;
        private Action _animationEndCallback;
        protected virtual void Awake()
        {
            Debug.Assert(_showAnimation);
        }

        public void PlayAnimation(Action callback = null)
        {
            _showAnimation.Play(_animationName);
            _animationEndCallback = callback;

        }

        public virtual void AnimationEndTrigger()
        {
            _animationEndCallback?.Invoke();
        }

    }

}