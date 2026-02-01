/*
 * ============================================================
 * TextAnimationDemo.cs
 * Interactive demo — attach to any GameObject in the scene.
 * ============================================================
 * 
 * Creates a simple on-screen UI (via Unity's legacy GUI) that
 * lets you toggle each animation type and tweak parameters live.
 * 
 * Setup:
 *     1. Create a Canvas → Text (TextMeshPro).
 *     2. Attach TextAnimationController to the TMP object.
 *     3. Attach this script to any GameObject (e.g. the Canvas).
 *     4. Drag the TMP object into the 'Target Text' field.
 *     5. Hit Play and use the on-screen panel.
 * ============================================================
 */

using UnityEngine;
using TMPro;

public class TextAnimationDemo : MonoBehaviour
{
    [Header("Target")]
    public TextMeshProUGUI targetText;

    // ── Live toggles ─────────────────────────────────────────────
    private bool _shake = true;
    private bool _jitter = true;
    private bool _wave = true;
    private bool _bounce = false;
    private bool _pulse = false;
    private bool _fade = false;

    // ── Live parameters ──────────────────────────────────────────
    private float _shakeAmp = 2f;
    private float _shakeFreq = 25f;
    private float _jitterAmp = 1.5f;
    private float _jitterFreq = 30f;
    private float _waveAmp = 3f;
    private float _waveFreq = 2f;
    private float _waveLen = 8f;

    // ─────────────────────────────────────────────────────────────

    private void RebuildText()
    {
        if (targetText == null) return;

        // Build the rich-text string dynamically from the current toggles.
        string inner = "Hello, TMP Animations!";

        // Wrap in tags from inside out so they nest properly.
        if (_fade) inner = $"<fade speed=\"{_waveFreq}\">{inner}</fade>";
        if (_pulse) inner = $"<pulse frequency=\"2\">{inner}</pulse>";
        if (_bounce) inner = $"<bounce>{inner}</bounce>";
        if (_wave) inner = $"<wave speed=\"{_waveFreq}\" wavelength=\"{_waveLen}\">{inner}</wave>";
        if (_jitter) inner = $"<jitter amplitude=\"{_jitterAmp}\" frequency=\"{_jitterFreq}\">{inner}</jitter>";
        if (_shake) inner = $"<shake amplitude=\"{_shakeAmp}\" frequency=\"{_shakeFreq}\">{inner}</shake>";

        targetText.text = inner;
    }

    // ─── OnGUI Panel ─────────────────────────────────────────────

    private void OnGUI()
    {
        // Semi-transparent background panel.
        var panelRect = new Rect(10, 10, 260, 340);
        GUI.color = new Color(0.05f, 0.05f, 0.1f, 0.85f);
        GUI.Box(panelRect, "");
        GUI.color = Color.white;

        float x = 20, y = 20, w = 240;
        GUI.Label(new Rect(x, y, w, 24),
            "<b><size=14><color=#88ccff>TMP Animation Demo</color></size></b>",
            CreateRichStyle());
        y += 28;

        // ── Shake ──
        y = ToggleRow(x, y, w, ref _shake, "Shake");
        if (_shake)
        {
            y = SliderRow(x, y, w, ref _shakeAmp, "  Amplitude", 0.5f, 8f);
            y = SliderRow(x, y, w, ref _shakeFreq, "  Frequency", 5f, 60f);
        }

        // ── Jitter ──
        y = ToggleRow(x, y, w, ref _jitter, "Jitter");
        if (_jitter)
        {
            y = SliderRow(x, y, w, ref _jitterAmp, "  Amplitude", 0.5f, 6f);
            y = SliderRow(x, y, w, ref _jitterFreq, "  Frequency", 10f, 60f);
        }

        // ── Wave ──
        y = ToggleRow(x, y, w, ref _wave, "Wave");
        if (_wave)
        {
            y = SliderRow(x, y, w, ref _waveAmp, "  Amplitude", 1f, 8f);
            y = SliderRow(x, y, w, ref _waveFreq, "  Speed", 0.5f, 6f);
            y = SliderRow(x, y, w, ref _waveLen, "  Wavelength", 2f, 20f);
        }

        // ── Bounce ──
        y = ToggleRow(x, y, w, ref _bounce, "Bounce");

        // ── Pulse ──
        y = ToggleRow(x, y, w, ref _pulse, "Pulse");

        // ── Fade ──
        y = ToggleRow(x, y, w, ref _fade, "Fade");

        // Rebuild whenever any toggle changes.
        RebuildText();
    }

    // ─── GUI Helpers ─────────────────────────────────────────────

    private float ToggleRow(float x, float y, float w, ref bool value, string label)
    {
        value = GUI.Toggle(new Rect(x, y, 20, 22), value, "");
        GUI.Label(new Rect(x + 22, y, w - 22, 22), label);
        return y + 24;
    }

    private float SliderRow(float x, float y, float w, ref float value, string label, float min, float max)
    {
        GUI.Label(new Rect(x, y, 90, 20), label);
        value = GUI.HorizontalSlider(new Rect(x + 92, y + 2, w - 110, 16), value, min, max);
        GUI.Label(new Rect(x + w - 36, y, 36, 20), value.ToString("F1"));
        return y + 22;
    }

    private GUIStyle CreateRichStyle()
    {
        var style = new GUIStyle(GUI.skin.label);
        style.richText = true;
        return style;
    }
}