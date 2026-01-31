using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project_Unorder.DialogueSystem
{
    public abstract class SpriteAnimation : TagAnimation
    {
        protected SpriteRenderer _spriteRenderer;
        protected Animator _animator;

        public void Init(SpriteRenderer spriteRenderer)
        {
            _spriteRenderer = spriteRenderer;
            _animator = _spriteRenderer.GetComponent<Animator>();
        }
    }
}
