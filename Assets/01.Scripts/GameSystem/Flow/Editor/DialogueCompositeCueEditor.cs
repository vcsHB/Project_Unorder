using UnityEditor;
using UnityEngine;

namespace Project_Unorder.FlowSystem
{
    [CustomEditor(typeof(DialogueCompositeCue))]
    public class DialogueCompositeCueEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var script = (DialogueCompositeCue)target;

            if (script.DialogueTable == null || string.IsNullOrEmpty(script.Key))
                return;

            if (script.StartStepIndex > script.EndStepIndex)
            {
                EditorGUILayout.HelpBox("Start Index must be less than or equal to End Index.", MessageType.Warning);
                return;
            }

            EditorGUILayout.Space(15);

            GUIStyle headerStyle = new GUIStyle(EditorStyles.boldLabel);
            headerStyle.normal.textColor = Color.white;

            GUIStyle boxStyle = new GUIStyle(EditorStyles.helpBox);
            boxStyle.padding = new RectOffset(10, 10, 10, 10);

            GUIStyle nameStyle = new GUIStyle(EditorStyles.label);
            nameStyle.richText = true;
            nameStyle.fontStyle = FontStyle.Bold;

            GUIStyle contentStyle = new GUIStyle(EditorStyles.wordWrappedLabel);
            contentStyle.richText = true;

            EditorGUILayout.LabelField("Dialogue Preview", headerStyle);
            EditorGUILayout.BeginVertical(boxStyle);

            string speakerName = script.Character != null
                ? script.Character.characterName.GetLocalizedString()
                : "Unknown";

            for (uint i = script.StartStepIndex; i <= script.EndStepIndex; i++)
            {
                string fullKey = $"{script.Key}_{script.ChapterIndex}_{i}";
                var entry = script.DialogueTable.GetEntry(fullKey);

                string content = entry != null
                    ? entry.LocalizedValue
                    : "<color=#FF0000>MISSING KEY</color>";

                EditorGUILayout.BeginVertical();
                EditorGUILayout.LabelField($"<color=#32CD32>[{speakerName}]</color> <color=#888888>({fullKey})</color>", nameStyle);
                EditorGUILayout.SelectableLabel(content, contentStyle,
                    GUILayout.Height(Mathf.Max(22, contentStyle.CalcHeight(new GUIContent(content), EditorGUIUtility.currentViewWidth - 60))));
                EditorGUILayout.EndVertical();

                if (i < script.EndStepIndex)
                {
                    EditorGUILayout.Space(2);
                    Rect rect = EditorGUILayout.GetControlRect(false, 1);
                    EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 0.3f));
                    EditorGUILayout.Space(5);
                }
            }

            EditorGUILayout.EndVertical();
        }
    }
}