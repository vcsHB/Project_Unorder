using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

public enum AnimationType { Shake, Jitter, Wave, Bounce, Pulse, Fade }

public sealed class AnimationRange
{
    public AnimationType Type;
    public int StartIndex;
    public int EndIndex;
    public float Amplitude = 2f;
    public float Frequency = 25f;
    public float WaveLength = 8f;
}

public static class TagParser
{
    private static readonly Regex OpenTagRegex = new(@"<(shake|jitter|wave|bounce|pulse|fade)(\s[^>]*)?>", RegexOptions.Compiled);
    private static readonly Regex CloseTagRegex = new(@"</(shake|jitter|wave|bounce|pulse|fade)>", RegexOptions.Compiled);
    private static readonly Regex AttributeRegex = new(@"(\w+)=""([^""]*)""", RegexOptions.Compiled);

    public static (string cleanText, List<AnimationRange> ranges) Parse(string rawText, float defShakeAmp, float defShakeFreq, float defJitterAmp, float defJitterFreq)
    {
        var ranges = new List<AnimationRange>();
        var stack = new Stack<(string name, int start, Dictionary<string, string> attrs)>();
        var cleanTextBuilder = new StringBuilder();

        int visibleIndex = 0;
        int pos = 0;

        while (pos < rawText.Length)
        {
            var openMatch = OpenTagRegex.Match(rawText, pos);
            var closeMatch = CloseTagRegex.Match(rawText, pos);

            if (openMatch.Success && openMatch.Index == pos)
            {
                var attrs = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                foreach (Match m in AttributeRegex.Matches(openMatch.Groups[2].Value))
                    attrs[m.Groups[1].Value] = m.Groups[2].Value;

                stack.Push((openMatch.Groups[1].Value, visibleIndex, attrs));
                pos += openMatch.Length;
            }
            else if (closeMatch.Success && closeMatch.Index == pos)
            {
                if (stack.Count > 0 && stack.Peek().name == closeMatch.Groups[1].Value)
                {
                    var info = stack.Pop();
                    var range = new AnimationRange { Type = Enum.Parse<AnimationType>(info.name, true), StartIndex = info.start, EndIndex = visibleIndex - 1 };

                    if (range.Type == AnimationType.Shake) { range.Amplitude = defShakeAmp; range.Frequency = defShakeFreq; }
                    else if (range.Type == AnimationType.Jitter) { range.Amplitude = defJitterAmp; range.Frequency = defJitterFreq; }

                    if (info.attrs.TryGetValue("amplitude", out var a) && float.TryParse(a, out float av)) range.Amplitude = av;
                    if (info.attrs.TryGetValue("frequency", out var f) && float.TryParse(f, out float fv)) range.Frequency = fv;
                    if (info.attrs.TryGetValue("speed", out var s) && float.TryParse(s, out float sv)) range.Frequency = sv;
                    if (info.attrs.TryGetValue("wavelength", out var w) && float.TryParse(w, out float wv)) range.WaveLength = wv;

                    ranges.Add(range);
                }
                pos += closeMatch.Length;
            }
            else
            {
                cleanTextBuilder.Append(rawText[pos]);
                visibleIndex++;
                pos++;
            }
        }
        return (cleanTextBuilder.ToString(), ranges);
    }
}