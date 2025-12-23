using MINISoundManage;
using UnityEngine;
using UnityEngine.Localization;
namespace Project_Unorder.DialogueSystem
{

    [System.Serializable]
    public class DialogueData
    {
        public Sprite profile;
        public string teller;
        public LocalizedString dialogueContent;
        public SoundSO typingSFX;
    }
}