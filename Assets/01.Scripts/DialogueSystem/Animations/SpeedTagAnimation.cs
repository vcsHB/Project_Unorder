using UnityEngine;

namespace Project_Unorder.DialogueSystem
{
    public class SpeedTagAnimation : StopReadingAnimation
    {
        private float _originSpeed;
        private float _speed;

        public SpeedTagAnimation()
        {
            _checkEndPos = true;
        }

        public override void OnStartTag()
        {
            _originSpeed = _dialogueBox.TextOutDelay;
            _dialogueBox.SetTextOutDelay(_speed);
        }

        public override void OnEndTag()
        {
            _dialogueBox.SetTextOutDelay(_originSpeed);
        }

        public override bool SetParameter()
        {
            if (float.TryParse(Param, out _speed) == false)
            {
                Debug.LogError($"{tagType.ToString()} ({Param}) : Parameter is wrong");
                return false;
            }
            return true;
        }

        public override void Complete() { }
    }
}
