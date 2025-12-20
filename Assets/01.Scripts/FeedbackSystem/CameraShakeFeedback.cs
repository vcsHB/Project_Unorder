using Project_Unorder.CameraSystem;
using UnityEngine;
namespace Project_Unorder.FeedbackSystem
{

    public class CameraShakeFeedback : Feedback
    {
        private CameraShakeComponent _shaker;
        [SerializeField] private float _shakeLevel;
        [SerializeField] private float _duration = 0.2f;

        private void Awake()
        {
            _shaker = CameraManager.Instance.GetCompo<CameraShakeComponent>();
            Debug.Assert(_shaker);
        }

        public override void CreateFeedback()
        {
            _shaker.Shake(_shakeLevel, _duration);
        }

        public override void FinishFeedback()
        {
        }
    }
}