using MINISoundManage;
using UnityEngine;

namespace Project_Unorder.FlowSystem
{
    public class MusicControlCue : FlowCue
    {
        [SerializeField] private SoundSO _newMusic;

        public override void Execute()
        {
            SoundController.Instance.MusicPlayer.ChangeMusic(_newMusic, true);
            InvokeCueComplete();
        }
    }
}