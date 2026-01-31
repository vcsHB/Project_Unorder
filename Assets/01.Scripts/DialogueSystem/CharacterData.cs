using UnityEngine;
using UnityEngine.Localization;

namespace Project_Unorder.DialogueSystem
{
    [CreateAssetMenu(menuName = "SO/DialogueSystem/CharacterData")]
    public class CharacterData : ScriptableObject
    {
        public LocalizedString characterName; 
        public Sprite icon;
    }
}