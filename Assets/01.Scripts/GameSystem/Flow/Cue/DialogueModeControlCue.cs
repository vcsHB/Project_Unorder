using Project_Unorder.DialogueSystem;
using UnityEngine;
namespace Project_Unorder.FlowSystem
{

    public class DialogueModeControlCue : FlowCue
    {
        [SerializeField] private bool _modeToggle = true;
        public override void Execute()
        {
            GlobalDialogueChannel.SetDialogueMode(_modeToggle);
            InvokeCueComplete();
        }
    }
}