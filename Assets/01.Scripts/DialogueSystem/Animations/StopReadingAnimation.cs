using System.Collections;
using System.Collections.Generic;
using Project_Unorder.DialogueSystem;
using UnityEngine;

namespace Project_Unorder.DialogueSystem
{
    public abstract class StopReadingAnimation : TagAnimation
    {
        protected DialogueBox _dialogueBox;

        public virtual void Init(DialogueBox box)
        {
            _dialogueBox = box;
        }
    }
}
