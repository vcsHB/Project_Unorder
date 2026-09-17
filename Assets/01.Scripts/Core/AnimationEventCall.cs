using UnityEngine;
using System;

public class AnimationEventCall : MonoBehaviour
{
    public event Action<int> OnAnimationTriggerBySequence;
    public event Action<string> OnAnimationTriggerByKey;

    public void CallEventBySequence(int sequenceIndex)
    {
        if (sequenceIndex < 0)
        {
            Debug.LogError("Animation EventCall By SequenceIndex is Invalid!");
            return;
        }
        OnAnimationTriggerBySequence?.Invoke(sequenceIndex);
    }

    public void CallEventByKey(string key)
    {
        if (key.Length == 0)
        {
            Debug.LogError("Animation EventCall By Key is Empty!");
            return;
        }
        OnAnimationTriggerByKey?.Invoke(key);
    }
}