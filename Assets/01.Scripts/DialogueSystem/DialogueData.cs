using MINISoundManage;
using UnityEngine;
namespace Project_Unorder.DialogueSystem
{
    [System.Serializable]
    public struct DialogueData
    {
        public Sprite profile;
        public string teller;
        public string content;
        public SoundSO typingSFX;
    }
}