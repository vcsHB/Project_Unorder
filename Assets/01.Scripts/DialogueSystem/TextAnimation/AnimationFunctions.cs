/*
 * ============================================================
 * AnimationFunctions.cs
 * Pure, stateless math functions for each animation type.
 * ============================================================
 * 
 * Every function is static and side-effect-free so they can be
 * composed, previewed, or unit-tested independently of Unity's
 * scene graph.
 * 
 * Convention:
 *     t         – world time (seconds)
 *     seed      – per-character random offset
 *     amplitude – displacement magnitude (world units)
 *     frequency – oscillation rate (Hz)
 * ============================================================
 */

using UnityEngine;

public static class AnimationFunctions
{
    // ─── Shake ───────────────────────────────────────────────────
    /// <summary>
    /// Smooth shake using layered sine waves at different phases.
    /// Produces a convincing vibration without harsh random snaps.
    /// </summary>
    public static Vector3 Shake(float t, float seed, float amplitude, float frequency)
    {
        float freqScaled = frequency;

        // Layer two sine waves at slightly offset frequencies for
        // visual complexity without needing a true RNG per frame.
        float x = Mathf.Sin(t * freqScaled + seed)
                + 0.5f * Mathf.Sin(t * freqScaled * 1.7f + seed * 2.3f);

        float y = Mathf.Cos(t * freqScaled + seed * 1.5f)
                + 0.5f * Mathf.Cos(t * freqScaled * 2.1f + seed * 0.7f);

        // Normalise so the combined signal stays within [-1, 1].
        x *= 0.5f;
        y *= 0.5f;

        return new Vector3(x * amplitude, y * amplitude, 0f);
    }

    // ─── Jitter ──────────────────────────────────────────────────
    /// <summary>
    /// High-frequency jitter.  Uses a pseudo-random hash of time
    /// quantised into discrete steps so the displacement *snaps*
    /// between positions rather than smoothly interpolating — giving
    /// the classic "vibrating" feel.
    /// </summary>
    public static Vector3 Jitter(float t, float seed, float amplitude, float frequency)
    {
        // Quantise time into discrete ticks.
        float tick = Mathf.Floor(t * frequency);

        float x = Hash(tick + seed)       * 2f - 1f;
        float y = Hash(tick + seed + 100f) * 2f - 1f;

        return new Vector3(x * amplitude, y * amplitude, 0f);
    }

    // ─── Wave ────────────────────────────────────────────────────
    /// <summary>
    /// Sinusoidal wave that propagates along the text string.
    /// Each character is offset in phase by its index, creating a
    /// rolling wave effect.
    /// </summary>
    public static Vector3 Wave(float t, float seed, float amplitude, float frequency, int charIndex, float waveLength)
    {
        float phase = (charIndex / waveLength) * Mathf.PI * 2f;
        float y = Mathf.Sin(t * frequency + phase) * amplitude;

        return new Vector3(0f, y, 0f);
    }

    // ─── Bounce ──────────────────────────────────────────────────
    /// <summary>
    /// Each character bounces up and down with a staggered delay
    /// based on its index, producing a playful "typing" feel.
    /// </summary>
    public static Vector3 Bounce(float t, float seed, float amplitude, float frequency)
    {
        // Stagger each character by its seed so they don't all
        // bounce in unison.
        float phase = seed * 0.1f;
        float raw   = Mathf.Sin((t * frequency + phase) * Mathf.PI);

        // Clamp negative half of sine to zero so the character only
        // moves upward (sits on a baseline).
        float y = Mathf.Max(raw, 0f) * amplitude;

        return new Vector3(0f, y, 0f);
    }

    // ─── Pulse ───────────────────────────────────────────────────
    /// <summary>
    /// Returns zero displacement (movement is handled via colour).
    /// The alpha value pulses smoothly between 0.3 and 1.0.
    /// </summary>
    public static Vector3 Pulse(float t, float seed, float amplitude, float frequency)
    {
        // No positional movement — see PulseAlpha.
        return Vector3.zero;
    }

    /// <summary>Alpha multiplier for the Pulse effect.</summary>
    public static float PulseAlpha(float t, float seed, float frequency)
    {
        float raw = Mathf.Sin(t * frequency + seed) * 0.5f + 0.5f; // [0, 1]
        return Mathf.Lerp(0.3f, 1f, raw);
    }

    // ─── Fade ────────────────────────────────────────────────────
    /// <summary>
    /// Characters fade in one-by-one with a staggered delay.
    /// Once fully visible, they stay visible.  The <fade> tag is
    /// typically applied once after the text is set.
    /// </summary>
    public static Vector3 Fade(float t, float seed, float amplitude, float frequency)
    {
        return Vector3.zero; // positional: none
    }

    /// <summary>
    /// Alpha for the Fade effect.  Each character's fade starts at
    /// a time offset proportional to its index.
    /// </summary>
    public static float FadeAlpha(float t, float seed, float frequency, int charIndex)
    {
        // Each character starts fading 'delay' seconds after the
        // previous one.  frequency here controls chars-per-second.
        float delay  = charIndex / Mathf.Max(frequency, 0.01f);
        float elapsed = t - delay;

        // Smooth step from 0 → 1 over 0.4 seconds once the delay has passed.
        float alpha = Mathf.SmoothStep(0f, 0.4f, elapsed);
        return Mathf.Clamp01(alpha);
    }

    // ─── Utility ─────────────────────────────────────────────────

    /// <summary>
    /// Fast deterministic hash mapping a float → [0, 1].
    /// Based on a common GLSL noise trick adapted for C#.
    /// </summary>
    private static float Hash(float value)
    {
        // Mix the bits.
        float x = value * 127.1f;
        x = Mathf.Sin(x) * 43758.5453f;
        // Fractional part gives pseudo-random [0,1).
        return x - Mathf.Floor(x);
    }
}
