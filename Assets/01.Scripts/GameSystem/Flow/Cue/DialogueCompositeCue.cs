using Core.TextUtil;
using Project_Unorder.DialogueSystem;
using UnityEngine;
using UnityEngine.Localization.Tables;

namespace Project_Unorder.FlowSystem
{
    public class DialogueCompositeCue : FlowCue
    {
#if UNITY_EDITOR
        [SerializeField] private StringTable _dialogueTable;
        public StringTable DialogueTable => _dialogueTable;
        public CharacterData Character => _character;
#endif
        [SerializeField] private CharacterData _character;
        [SerializeField] private string _key;
        [SerializeField] private uint _chapterIndex;
        [SerializeField] private uint _startStepIndex;
        [SerializeField] private uint _endStepIndex;
        [SerializeField] private bool _isCharacterExit;

        public string Key => _key;
        public uint ChapterIndex => _chapterIndex;
        public uint StartStepIndex => _startStepIndex;
        public uint EndStepIndex => _endStepIndex;
        private uint _currentStepIndex;
        private bool _isStarted = false;
        public override bool Execute()
        {
            //if (_startStepIndex > _endStepIndex) return;
            if (!_isStarted)
            {
                _currentStepIndex = _startStepIndex;
                GlobalDialogueChannel.SetDialogueOwner(_character, true);
                _isStarted = true;
            }
            
            BroadCastDialogueText(_currentStepIndex);
            ++_currentStepIndex;
            if (_currentStepIndex > _endStepIndex)
            {
                _isStarted = false;
                InvokeCueComplete();
                return true;
            }
            return false;
        }

        private void BroadCastDialogueText(uint index)
        {
            DialogueLocalizedData data = new DialogueLocalizedData()
            {
                content = TextUtil.Instance.GetText(TextType.Dialogue, $"{_key}_{_chapterIndex}_{index}")
            };
            GlobalDialogueChannel.BroadCast(ref data);
        }
    }
}