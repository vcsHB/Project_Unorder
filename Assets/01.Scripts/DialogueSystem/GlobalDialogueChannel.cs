using System;
using UnityEngine;
namespace Project_Unorder.DialogueSystem
{
    public delegate void OnDialoguePrintEvent(ref DialogueLocalizedData dialogueBlock);
    public static class GlobalDialogueChannel
    {
        public static event Action<bool> OnDialogueToggleEvent;
        public static event Action<CharacterData, bool> OnDialogueBoxOwnerToggleEvent;
        private static OnDialoguePrintEvent OnDialogueBroadcast;

        public static void AddListener(OnDialoguePrintEvent action)
        {
            OnDialogueBroadcast += action;
        }
        public static void RemoveListener(OnDialoguePrintEvent action)
        {
            OnDialogueBroadcast -= action;
        }

        public static void SetDialogueMode(bool value)
        {
            OnDialogueToggleEvent?.Invoke(value);
        }

        public static void SetDialogueOwner(CharacterData characterData, bool value)
        {
            OnDialogueBoxOwnerToggleEvent?.Invoke(characterData, value);
        }

        public static void BroadCast(ref DialogueLocalizedData dialogueBlock)
        {
            OnDialogueBroadcast?.Invoke(ref dialogueBlock);
        }

        public static void ClearListeners()
        {
            OnDialogueBroadcast = null;

        }

    }
}