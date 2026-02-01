using Project_Unorder.InputManage;
using UnityEngine;
namespace Project_Unorder.DialogueSystem
{

    public class DialogueController : MonoBehaviour
    {
        [SerializeField] private DialogueBox _dialogueBox;
        [SerializeField] private UIInput _uiInput;
        private bool _isDialogueMode;
        private void Awake()
        {
            GlobalDialogueChannel.OnDialogueToggleEvent += HandleDialogueToggle;
            GlobalDialogueChannel.OnDialogueBoxOwnerToggleEvent += HandleDialogueToggle;
            GlobalDialogueChannel.AddListener(HandleDialogueListen);

            _uiInput.OnSubmitEvent += HandleContinueDialogue;
        }

        private void OnDestroy()
        {

            GlobalDialogueChannel.OnDialogueToggleEvent -= HandleDialogueToggle;
            GlobalDialogueChannel.RemoveListener(HandleDialogueListen);
            _uiInput.OnSubmitEvent -= HandleContinueDialogue;

        }

        private void HandleContinueDialogue()
        {
            if (_isDialogueMode)
            {
                GlobalDialogueChannel.ContinueDialogue();
            }
        }

        private void HandleDialogueListen(ref DialogueLocalizedData data)
        {
            _dialogueBox.StartSpeech(ref data);
        }

        private void HandleDialogueToggle(CharacterData data, bool value)
        {
            _dialogueBox.Open();
            _dialogueBox.SetOwner(data);

        }

        private void HandleDialogueToggle(bool toggleValue)
        {
            _isDialogueMode = toggleValue;
            if (toggleValue)
            {
                _dialogueBox.Open();
            }
            else
            {
                _dialogueBox.Close();
            }
        }
    }
}