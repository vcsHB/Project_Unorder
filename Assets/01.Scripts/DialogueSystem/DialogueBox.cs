using System;
using System.Collections;
using MINISoundManage;
using TMPro;
using UnityEngine;
namespace Project_Unorder.DialogueSystem
{

    public class DialogueBox : MonoBehaviour
    {
        public event Action OnSpeechOverEvent;
        [SerializeField] private CharacterData _owner;

        [Header("Essential Settings")]
        [SerializeField] private TextMeshProUGUI _ownerNameText;
        [SerializeField] private TextMeshProUGUI _contentText;
        [SerializeField] private float _textOutDelay = 0.1f;

        [SerializeField] private bool _isReadingDialogue;
        public bool StopReading { get; set; }
        public float TextOutDelay => _textOutDelay;
        private string _content;
        private SoundSO _typingSFX;


        public void SetTextOutDelay(float outDelay)
        {
            _textOutDelay = outDelay;
        }

        public void SetOwner(CharacterData owner)
        {
            _owner = owner;
            _ownerNameText.text = owner.characterName.GetLocalizedString();
        }

        public void StartSpeech(ref DialogueLocalizedData data)
        {
            _content = data.content.GetLocalizedString();
            _typingSFX = data.typingSFX;
            StartCoroutine(ReadingNormalNodeRoutine());

        }

        protected virtual IEnumerator ReadingNormalNodeRoutine()
        {
            _contentText.maxVisibleCharacters = 0;
            _contentText.text = _content;
            //InitNodeAnim(node);
            _isReadingDialogue = true;
            while (_contentText.maxVisibleCharacters < _contentText.text.Length)
            {
                if (_contentText.text[_contentText.maxVisibleCharacters++] == ' ') continue;

                if (_typingSFX != null)
                    SoundController.Instance.PlaySound(_typingSFX, transform.position);

                yield return new WaitForSeconds(_textOutDelay);
                yield return new WaitUntil(() => StopReading == false);
            }
            OnSpeechOverEvent?.Invoke();
        }


    }
}