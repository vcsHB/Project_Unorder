using Project_Unorder.InputManage;
using UnityEngine;
namespace Project_Unorder.FlowSystem
{

    public class InputConfigControlCue : FlowCue
    {
        [SerializeField] private InputConfigPresetEnum _presetType;

        public override void Execute()
        {
            GlobalInputConfig.SetPreset(_presetType);
            InvokeCueComplete();
        }

    }
}