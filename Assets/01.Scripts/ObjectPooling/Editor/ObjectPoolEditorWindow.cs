using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace ObjectPooling.Editor
{
    public class ObjectPoolEditorWindow : EditorWindow
    {
        private const string EnumNamespace = "ObjectPooling.Generated";
        private const string GeneratedFolder = "Assets/01.Scripts/ObjectPooling/Generated";

        private List<PoolGroupSO> _groups = new();
        private Vector2 _scrollPos;
        private readonly Dictionary<PoolGroupSO, bool> _foldouts = new();
        private bool _dirty;

        [MenuItem("Tools/Object Pool Manager")]
        public static void Open() => GetWindow<ObjectPoolEditorWindow>("Object Pool Manager");

        private void OnEnable() => RefreshGroups();

        private void RefreshGroups()
        {
            _groups.Clear();
            var guids = AssetDatabase.FindAssets("t:PoolGroupSO");
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var so = AssetDatabase.LoadAssetAtPath<PoolGroupSO>(path);
                if (so != null) _groups.Add(so);
            }
        }

        private void OnGUI()
        {
            DrawToolbar();

            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

            if (_groups.Count == 0)
            {
                EditorGUILayout.HelpBox("No Pool Groups found. Create one using the button above.", MessageType.Info);
            }

            for (int g = 0; g < _groups.Count; g++)
                DrawGroup(_groups[g], g);

            EditorGUILayout.EndScrollView();

            if (_dirty)
            {
                EditorGUILayout.HelpBox("Unsaved changes. Press 'Save All' or use Generate Enum.", MessageType.Warning);
                if (GUILayout.Button("Save All", GUILayout.Height(30)))
                {
                    AssetDatabase.SaveAssets();
                    _dirty = false;
                }
            }
        }

        private void DrawToolbar()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);

            if (GUILayout.Button("+ New Pool Group", EditorStyles.toolbarButton, GUILayout.Width(140)))
                CreateNewGroup();

            if (GUILayout.Button("Refresh", EditorStyles.toolbarButton, GUILayout.Width(70)))
                RefreshGroups();

            if (GUILayout.Button("Generate All Enums", EditorStyles.toolbarButton, GUILayout.Width(140)))
                GenerateAllEnums();

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }

        private void DrawGroup(PoolGroupSO group, int index)
        {
            if (!_foldouts.ContainsKey(group)) _foldouts[group] = true;

            var bgColor = index % 2 == 0 ? new Color(0.22f, 0.22f, 0.22f) : new Color(0.19f, 0.19f, 0.19f);
            var rect = EditorGUILayout.BeginVertical();
            EditorGUI.DrawRect(rect, bgColor);

            // Header
            EditorGUILayout.BeginHorizontal();
            _foldouts[group] = EditorGUILayout.Foldout(_foldouts[group], GUIContent.none, true);

            EditorGUI.BeginChangeCheck();
            group.groupName = EditorGUILayout.TextField(group.groupName, EditorStyles.boldLabel, GUILayout.ExpandWidth(true));
            if (EditorGUI.EndChangeCheck()) MarkDirty(group);

            var enumStatus = string.IsNullOrEmpty(group.enumTypeName) ? "⚠ No Enum" : "✓ Enum OK";
            var statusStyle = new GUIStyle(EditorStyles.miniLabel)
                { normal = { textColor = string.IsNullOrEmpty(group.enumTypeName) ? Color.yellow : Color.green } };
            GUILayout.Label(enumStatus, statusStyle, GUILayout.Width(90));

            if (GUILayout.Button("Generate Enum", EditorStyles.miniButton, GUILayout.Width(105)))
                GenerateEnum(group);

            if (GUILayout.Button("✕", EditorStyles.miniButton, GUILayout.Width(22)))
            {
                if (EditorUtility.DisplayDialog("Delete Pool Group",
                    $"Delete '{group.groupName}'? This will delete the SO asset.", "Delete", "Cancel"))
                {
                    DeleteGroup(group);
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.EndVertical();
                    return;
                }
            }
            EditorGUILayout.EndHorizontal();

            if (!_foldouts[group])
            {
                EditorGUILayout.EndVertical();
                return;
            }

            EditorGUI.indentLevel++;

            // Items header
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Items", EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("+ Add Item", EditorStyles.miniButton, GUILayout.Width(80)))
            {
                group.items.Add(new PoolItemData());
                MarkDirty(group);
            }
            EditorGUILayout.EndHorizontal();

            for (int i = 0; i < group.items.Count; i++)
                DrawItem(group, i);

            EditorGUI.indentLevel--;
            GUILayout.Space(4);
            EditorGUILayout.EndVertical();
        }

        private void DrawItem(PoolGroupSO group, int index)
        {
            var item = group.items[index];

            var itemRect = EditorGUILayout.BeginVertical("box");

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label($"#{index}", GUILayout.Width(24));

            EditorGUI.BeginChangeCheck();

            item.itemName = EditorGUILayout.TextField(item.itemName, GUILayout.Width(130));

            EditorGUILayout.LabelField("Count", GUILayout.Width(40));
            item.initialCount = EditorGUILayout.IntField(item.initialCount, GUILayout.Width(50));

            item.prefab = (GameObject)EditorGUILayout.ObjectField(item.prefab, typeof(GameObject), false);

            if (EditorGUI.EndChangeCheck()) MarkDirty(group);

            // Prefab validation
            if (item.prefab != null)
            {
                bool hasPoolable = item.prefab.GetComponent<IPoolable>() != null;
                var validStyle = new GUIStyle(EditorStyles.miniLabel)
                    { normal = { textColor = hasPoolable ? Color.green : Color.red } };
                GUILayout.Label(hasPoolable ? "✓" : "✗ IPoolable", validStyle, GUILayout.Width(hasPoolable ? 16 : 65));
            }

            if (GUILayout.Button("✕", EditorStyles.miniButton, GUILayout.Width(22)))
            {
                group.items.RemoveAt(index);
                MarkDirty(group);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
                return;
            }
            EditorGUILayout.EndHorizontal();

            // Description
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(28);
            item.description = EditorGUILayout.TextField("Description", item.description);
            EditorGUILayout.EndHorizontal();
            if (EditorGUI.EndChangeCheck()) MarkDirty(group);

            EditorGUILayout.EndVertical();
        }

        private void MarkDirty(PoolGroupSO group)
        {
            EditorUtility.SetDirty(group);
            _dirty = true;
        }

        private void CreateNewGroup()
        {
            var path = EditorUtility.SaveFilePanelInProject(
                "Create Pool Group", "NewPoolGroup", "asset", "Choose save location");
            if (string.IsNullOrEmpty(path)) return;

            var so = CreateInstance<PoolGroupSO>();
            so.groupName = Path.GetFileNameWithoutExtension(path);
            AssetDatabase.CreateAsset(so, path);
            AssetDatabase.SaveAssets();
            _groups.Add(so);
        }

        private void DeleteGroup(PoolGroupSO group)
        {
            _groups.Remove(group);
            _foldouts.Remove(group);
            var path = AssetDatabase.GetAssetPath(group);
            AssetDatabase.DeleteAsset(path);
            AssetDatabase.Refresh();
        }

        private void GenerateAllEnums()
        {
            foreach (var group in _groups)
                GenerateEnum(group);
        }

        private void GenerateEnum(PoolGroupSO group)
        {
            if (string.IsNullOrEmpty(group.groupName))
            {
                Debug.LogError("[ObjectPool] Group name is empty.");
                return;
            }

            // Validate items
            bool hasErrors = false;
            var seen = new HashSet<string>();
            foreach (var item in group.items)
            {
                if (string.IsNullOrWhiteSpace(item.itemName))
                {
                    Debug.LogError($"[ObjectPool] Group '{group.groupName}' has an item with an empty name.");
                    hasErrors = true;
                }
                else if (!seen.Add(item.itemName))
                {
                    Debug.LogError($"[ObjectPool] Duplicate item name '{item.itemName}' in group '{group.groupName}'.");
                    hasErrors = true;
                }
            }
            if (hasErrors) return;

            Directory.CreateDirectory(GeneratedFolder);

            var enumName = $"{SanitizeName(group.groupName)}PoolKey";
            var filePath = $"{GeneratedFolder}/{enumName}.cs";

            var sb = new StringBuilder();
            sb.AppendLine("// <auto-generated> Do not edit manually. Use Object Pool Manager window. </auto-generated>");
            sb.AppendLine($"namespace {EnumNamespace}");
            sb.AppendLine("{");
            sb.AppendLine($"    public enum {enumName}");
            sb.AppendLine("    {");
            for (int i = 0; i < group.items.Count; i++)
                sb.AppendLine($"        {SanitizeName(group.items[i].itemName)} = {i},");
            sb.AppendLine("    }");
            sb.AppendLine("}");

            File.WriteAllText(filePath, sb.ToString());

            group.enumTypeName = $"{EnumNamespace}.{enumName}, Assembly-CSharp";
            EditorUtility.SetDirty(group);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"[ObjectPool] Generated enum '{enumName}' at {filePath}");
        }

        private static string SanitizeName(string name)
        {
            if (string.IsNullOrEmpty(name)) return "_";
            var sb = new StringBuilder();
            foreach (var c in name)
            {
                if (char.IsLetterOrDigit(c)) sb.Append(c);
                else if (c == '_') sb.Append('_');
            }
            if (sb.Length > 0 && char.IsDigit(sb[0])) sb.Insert(0, '_');
            return sb.Length > 0 ? sb.ToString() : "_";
        }
    }
}
