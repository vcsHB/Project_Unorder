using System;
using System.Collections;
using System.Collections.Generic;
using MINISoundManage;
using Project_Unorder.UIManage.InGameSceneUI;
using TMPro;
using UnityEngine;
namespace Project_Unorder.DialogueSystem
{

    public class DialogueBox : AnimationWindowPanel
    {
        public event Action OnSpeechOverEvent;
        [SerializeField] private CharacterData _owner;

        [Header("Essential Settings")]
        [SerializeField] private AnimationWindowPanel _animationPanel;
        [SerializeField] private TextMeshProUGUI _ownerNameText;
        [SerializeField] private TextMeshProUGUI _contentText;
        [SerializeField] private float _textOutDelay = 0.1f;

        [SerializeField] private bool _isReadingDialogue;
        public bool StopReading { get; set; }
        public float TextOutDelay => _textOutDelay;

        protected bool _playingEndAnimation = false;
        public bool PlayingEndAnimation => _playingEndAnimation;
        private string _content;
        private SoundSO _typingSFX;

        protected override void Awake()
        {
            base.Awake();
        }


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
            _content = data.content;
            _typingSFX = data.typingSFX;
            StartCoroutine(ReadingTextRoutine());

        }
        public virtual void CompleteEndAnimation() => _playingEndAnimation = false;

        protected virtual IEnumerator ReadingTextRoutine()
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
            _isReadingDialogue = false;
        }


    }
}