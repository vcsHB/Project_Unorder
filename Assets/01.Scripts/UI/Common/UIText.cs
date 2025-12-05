using TMPro;
using UnityEngine;

namespace Project_Unorder.UIManage
{
#if UNITY_EDITOR
    using UnityEditor;
    public class UITextData
    {

        [MenuItem("CONTEXT/TextMeshProUGUI/Convert to UIText")]
        private static void Convert(MenuCommand command)
        {
            TextMeshProUGUI src = (TextMeshProUGUI)command.context;
            GameObject obj = src.gameObject;

            Undo.RecordObject(obj, "Convert TextMeshProUGUI To UIText");

            var backup = new TextData(src);
            Undo.DestroyObjectImmediate(src);
            var dst = Undo.AddComponent<UIText>(obj);
            backup.ApplyTo(dst);

            Debug.Log($"Converted TextMeshProUGUI → UIText ({obj.name})");
        }

        private struct TextData
        {

            public string text;
            public TMP_FontAsset font;
            public float fontSize;
            public Color color;
            public TextAlignmentOptions alignment;
            public TextWrappingModes textWrappingMode;
            public bool enableAutoSizing;
            public float characterSpacing;
            public float lineSpacing;

            public TextData(TextMeshProUGUI tmp)
            {
                text = tmp.text;
                font = tmp.font;
                fontSize = tmp.fontSize;
                color = tmp.color;
                alignment = tmp.alignment;
                textWrappingMode = tmp.textWrappingMode;
                enableAutoSizing = tmp.enableAutoSizing;
                characterSpacing = tmp.characterSpacing;
                lineSpacing = tmp.lineSpacing;
            }

            public void ApplyTo(TextMeshProUGUI tmp)
            {
                tmp.text = text;
                tmp.font = font;
                tmp.fontSize = fontSize;
                tmp.color = color;
                tmp.alignment = alignment;
                tmp.textWrappingMode = textWrappingMode;
                tmp.enableAutoSizing = enableAutoSizing;
                tmp.characterSpacing = characterSpacing;
                tmp.lineSpacing = lineSpacing;
            }
        }


    }

#endif

    public class UIText : TextMeshProUGUI, IColorChangeable
    {
        public void ChangeColor(Color newColor)
        {
            color = newColor;
        }


    }
}
