using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextAnimationController : MonoBehaviour
{
    public float defaultShakeAmplitude = 2f;
    public float defaultShakeFrequency = 25f;
    public float defaultJitterAmplitude = 1.5f;
    public float defaultJitterFrequency = 30f;

    private TextMeshProUGUI _tmp;
    private List<AnimationRange> _ranges = new();
    private Vector3[][] _preAnimVertices;
    private float[] _seeds;
    private string _lastRawText;
    private string _lastProcessedText;

    private void Awake() => _tmp = GetComponent<TextMeshProUGUI>();

    private void LateUpdate()
    {
        if (_tmp.text != _lastProcessedText && !string.IsNullOrEmpty(_tmp.text))
        {
            _ranges.Clear();

            if (_tmp.text.Contains("<"))
            {
                var result = TagParser.Parse(_tmp.text, defaultShakeAmplitude, defaultShakeFrequency, defaultJitterAmplitude, defaultJitterFrequency);
                _ranges = result.ranges;
                _lastRawText = result.cleanText;
                _tmp.text = _lastProcessedText = _lastRawText;
                _tmp.ForceMeshUpdate(true, true);
            }
            else
            {
                _lastProcessedText = _tmp.text;
                _lastRawText = _tmp.text;
            }
        }

        if (_ranges.Count == 0) return;

        _tmp.ForceMeshUpdate(false, false);
        TMP_TextInfo textInfo = _tmp.textInfo;
        BackupVertices(textInfo);

        float t = Time.time;
        if (_seeds == null || _seeds.Length < textInfo.characterCount) InitSeeds(textInfo.characterCount);

        foreach (var range in _ranges)
        {
            for (int i = range.StartIndex; i <= range.EndIndex && i < textInfo.characterCount; i++)
            {
                var charInfo = textInfo.characterInfo[i];
                if (!charInfo.isVisible) continue;

                Vector3 delta = range.Type switch
                {
                    AnimationType.Shake => AnimationFunctions.Shake(t, _seeds[i], range.Amplitude, range.Frequency),
                    AnimationType.Jitter => AnimationFunctions.Jitter(t, _seeds[i], range.Amplitude, range.Frequency),
                    AnimationType.Wave => AnimationFunctions.Wave(t, _seeds[i], range.Amplitude, range.Frequency, i, range.WaveLength),
                    AnimationType.Bounce => AnimationFunctions.Bounce(t, _seeds[i], range.Amplitude, range.Frequency),
                    _ => Vector3.zero
                };

                int mIdx = charInfo.materialReferenceIndex;
                int vIdx = charInfo.vertexIndex;
                for (int v = 0; v < 4; v++) textInfo.meshInfo[mIdx].vertices[vIdx + v] += delta;
            }
        }

        ApplyColourAnimations(t, textInfo);
        _tmp.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
    }

    private void ApplyColourAnimations(float t, TMP_TextInfo textInfo)
    {
        foreach (var range in _ranges)
        {
            if (range.Type != AnimationType.Fade && range.Type != AnimationType.Pulse) continue;

            for (int i = range.StartIndex; i <= range.EndIndex && i < textInfo.characterCount; i++)
            {
                var charInfo = textInfo.characterInfo[i];
                if (!charInfo.isVisible) continue;

                float alpha = range.Type switch
                {
                    AnimationType.Fade => AnimationFunctions.FadeAlpha(t, _seeds[i], range.Frequency, i),
                    AnimationType.Pulse => AnimationFunctions.PulseAlpha(t, _seeds[i], range.Frequency),
                    _ => 1f
                };

                byte aByte = (byte)Mathf.Clamp(alpha * 255f, 0, 255);
                int mIdx = charInfo.materialReferenceIndex;
                int vIdx = charInfo.vertexIndex;
                for (int v = 0; v < 4; v++) textInfo.meshInfo[mIdx].colors32[vIdx + v].a = aByte;
            }
        }
    }

    private void BackupVertices(TMP_TextInfo textInfo)
    {
        if (_preAnimVertices == null || _preAnimVertices.Length < textInfo.meshInfo.Length)
            _preAnimVertices = new Vector3[textInfo.meshInfo.Length][];

        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            int vCount = textInfo.meshInfo[i].vertices.Length;
            if (_preAnimVertices[i] == null || _preAnimVertices[i].Length < vCount)
                _preAnimVertices[i] = new Vector3[vCount];

            Array.Copy(textInfo.meshInfo[i].vertices, _preAnimVertices[i], vCount);
        }
    }

    private void InitSeeds(int count)
    {
        _seeds = new float[Mathf.Max(count, 100)];
        var rng = new System.Random(42);
        for (int i = 0; i < _seeds.Length; i++) _seeds[i] = (float)rng.NextDouble() * 1000f;
    }
}