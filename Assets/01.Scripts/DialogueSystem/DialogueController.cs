using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Project_Unorder.DialogueSystem
{

    public class DialogueController : MonoBehaviour
    {
        [SerializeField] private DialogueBox _dialogueBoxPrefab;
        private Queue<DialogueBox> _dialogueBoxPool = new();
        private void Awake()
        {
            InitializePool();

            GlobalDialogueChannel.OnDialogueToggleEvent += HandleDialogueToggle;
            GlobalDialogueChannel.OnDialogueBoxOwnerToggleEvent += HandleDialogueToggle;
            GlobalDialogueChannel.AddListener(HandleDialogueListen);
        }

        private void InitializePool()
        {
            if(_dialogueBoxPrefab == null)
            {
                Debug.LogError("DialogueBox Prefab is null");
                return;
            }
            
        }

        private void OnDestroy()
        {
            GlobalDialogueChannel.OnDialogueToggleEvent -= HandleDialogueToggle;
            GlobalDialogueChannel.RemoveListener(HandleDialogueListen);
        }

        private void HandleDialogueListen(ref DialogueLocalizedData data)
        {

        }

        private void HandleDialogueToggle(CharacterData data, bool value)
        {

        }

        private void HandleDialogueToggle(bool obj)
        {
            throw new NotImplementedException();
        }
    }
}