using System.Collections;
using UnityEngine;

namespace Project_Unorder.DialogueSystem
{
    public class WaitTagAnimation : StopReadingAnimation
    {
        private float _delay;
        private float _animStartTime;

        private bool animStartFlag = true;

        public WaitTagAnimation()
        {
            _timing = AnimTiming.OnTextOut;
            tagType = TagEnum.Wait;
            _stopReadingDuringAnimation = true;
            _checkEndPos = false;
        }

        public override void Play()
        {
            base.Play();
            var charInfo = _txtInfo.characterInfo[animStartPos - 1];

            if(charInfo.isVisible && animStartFlag)
            {
                animStartFlag = false;
                _animStartTime = Time.time;
                _dialogueBox.StopReading = true;
            }

            if (_animStartTime + _delay <= Time.time)
            {
                _dialogueBox.StopReading = false;
            }
        }

        public override void Init()
        {
            base.Init();
            animStartFlag = true;
        }

        public override void Complete()
        {

        }

        public override bool SetParameter()
        {
            if (float.TryParse(Param, out _delay) == false)
            {
                Debug.LogError($"{tagType.ToString()} ({Param}) : Parameter is wrong");

                return false;
            }
            return true;
        }
    }
}
