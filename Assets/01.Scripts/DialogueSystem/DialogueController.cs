using System;
using UnityEngine;
namespace Project_Unorder.DialogueSystem
{

    public class DialogueController : MonoBehaviour
    {
        private void Awake()
        {
            GlobalDialogueChannel.OnDialogueToggleEvent += HandleDialogueToggle;
            GlobalDialogueChannel.AddListener(HandleDialogueListen);
        }

        private void OnDestroy()
        {
            GlobalDialogueChannel.OnDialogueToggleEvent -= HandleDialogueToggle;
            GlobalDialogueChannel.RemoveListener(HandleDialogueListen);
        }

        private void HandleDialogueListen(ref DialogueLocalizedData data)
        {
            
        }

        private void HandleDialogueToggle(bool obj)
        {
            throw new NotImplementedException();
        }
    }
}