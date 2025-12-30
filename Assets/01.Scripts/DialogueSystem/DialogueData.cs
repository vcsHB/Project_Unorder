using MINISoundManage;
using UnityEngine;
using UnityEngine.Localization;
namespace Project_Unorder.DialogueSystem
{

    [System.Serializable]
    public class DialogueData : ScriptableObject
    {
        public Sprite profileSprite;
        public LocalizedString tellerName;
        public LocalizedString dialogueContent;
        public SoundSO typingSFX;
    }
}