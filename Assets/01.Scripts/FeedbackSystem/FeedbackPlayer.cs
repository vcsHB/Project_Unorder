using UnityEngine;
using UnityEngine.Events;

namespace Project_Unorder.FeedbackSystem
{

    public class FeedbackPlayer : MonoBehaviour
    {
        public UnityEvent OnFeedbackCreateEvent;
        public UnityEvent OnFeedbackStopEvent;
        private Feedback[] _feedbacks;
        private void Awake()
        {
            _feedbacks = GetComponents<Feedback>();
        }

        public void CreateFeedback()
        {
            if (_feedbacks == null) return;
            for (int i = 0; i < _feedbacks.Length; i++)
            {
                _feedbacks[i].CreateFeedback();
            }
            OnFeedbackCreateEvent?.Invoke();
        }

        public void StopFeedback()
        {
            for (int i = 0; i < _feedbacks.Length; i++)
            {
                _feedbacks[i].FinishFeedback();
            }
            OnFeedbackStopEvent?.Invoke();
        }
    }

}
