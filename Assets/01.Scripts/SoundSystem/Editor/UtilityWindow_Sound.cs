using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace MINISoundManage.EditorScripts
{
    public partial class UtilityWindow
    {
        private readonly string _soundDirectory = "Assets/08.SO/Sound";
        private SoundTableSO _soundTable;
        private string _newTableName = "NewSoundTable";

        private const string PREF_KEY_TABLE = "SoundManage_LastSelectedTable";

        private void OnEnable()
        {
            SetUpUtility();

            string lastPath = EditorPrefs.GetString(PREF_KEY_TABLE, string.Empty);
            if (!string.IsNullOrEmpty(lastPath))
            {
                _soundTable = AssetDatabase.LoadAssetAtPath<SoundTableSO>(lastPath);
            }
        }
        
        private void InputSoundTable()
        {
            EditorGUILayout.LabelField("Sound Table", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical();
            {
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUI.BeginChangeCheck();
                    _soundTable = (SoundTableSO)EditorGUILayout.ObjectField(
                        _soundTable, typeof(SoundTableSO), false, GUILayout.Width(200f)
                    );

                    if (_soundTable != null)
                    {
                        SerializedObject so = new SerializedObject(_soundTable);
                        SerializedProperty audioTypeProp = so.FindProperty("audioType");

                        EditorGUILayout.PropertyField(
                            audioTypeProp,
                            new GUIContent("Audio Type"),
                            GUILayout.Width(300f)
                        );

                        if (so.ApplyModifiedProperties())
                        {
                            EditorUtility.SetDirty(_soundTable);
                        }
                    }
                    EditorGUI.EndChangeCheck();
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space(10f);

                EditorGUILayout.BeginHorizontal();
                {
                    _newTableName = EditorGUILayout.TextField(_newTableName, GUILayout.Width(150f));

                    if (GUILayout.Button("Create Sound Table SO", GUILayout.Width(180f)))
                    {
                        _soundTable = CreateSoundTableSO(_newTableName);
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
        }




        private void DrawSoundItems()
        {
            InputSoundTable();

            if (_soundTable == null)
                return;
            EditorGUILayout.Space(10f);

            CreateSoundSODraw();

            GUI.color = Color.white;
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Width(300f));
                {
                    EditorGUILayout.LabelField("Sound List", EditorStyles.boldLabel);
                    EditorGUILayout.Space(3f);

                    scrollPositions[UtilType.Sound] = EditorGUILayout.BeginScrollView(
                        scrollPositions[UtilType.Sound], false, true,
                        GUIStyle.none, GUI.skin.verticalScrollbar, GUIStyle.none);
                    {
                        DrawSoundTable();
                    }
                    EditorGUILayout.EndScrollView();
                }
                EditorGUILayout.EndVertical();

                DrawSoundInspector();
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawSoundInspector()
        {
            if (selectedItem[UtilType.Sound] != null)
            {
                Vector2 scroll = Vector2.zero;
                EditorGUILayout.BeginScrollView(scroll);
                {
                    EditorGUILayout.Space(2f);
                    Editor.CreateCachedEditor(
                        selectedItem[UtilType.Sound], null, ref _cachedEditor);

                    _cachedEditor.OnInspectorGUI();
                }
                EditorGUILayout.EndScrollView();
            }
        }

        private void DrawSoundTable()
        {
            foreach (var soundSO in _soundTable.soundSOList)
            {
                GUIStyle style = selectedItem[UtilType.Sound] == soundSO
                    ? _selectStyle
                    : GUIStyle.none;

                EditorGUILayout.BeginHorizontal(style, GUILayout.Height(40f));
                {
                    EditorGUILayout.LabelField(soundSO.name, GUILayout.Width(240f), GUILayout.Height(40f));

                    EditorGUILayout.BeginVertical();
                    {
                        EditorGUILayout.Space(10f);
                        SoundDeleteButton(soundSO);
                    }
                    EditorGUILayout.EndVertical();
                }
                EditorGUILayout.EndHorizontal();
                if (soundSO == null)
                    break;

                GetRect(soundSO);
            }
        }

        private void SoundDeleteButton(SoundSO soundData)
        {
            GUI.color = Color.red;
            if (GUILayout.Button("X", GUILayout.Width(20f)))
            {
                Debug.Log($"[MINI Sound] Sound SO Data Deleted. Name:{soundData.soundName}");
                _soundTable.soundSOList.Remove(soundData);
                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(soundData));
                EditorUtility.SetDirty(_soundTable);
                AssetDatabase.SaveAssets();
            }
            GUI.color = Color.white;
        }

        private void GetRect(SoundSO soundSO)
        {
            Rect rect = GUILayoutUtility.GetLastRect();

            if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition))
            {
                inspectorScroll = Vector2.zero;
                selectedItem[UtilType.Sound] = soundSO;
                Event.current.Use();
            }
        }

        private void CreateSoundSODraw()
        {
            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Create Sound Data"))
                {
                    CreateSoundSO();
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        private SoundTableSO CreateSoundTableSO(string tableName)
        {
            SoundTableSO newTable = CreateInstance<SoundTableSO>();
            newTable.name = tableName;

            string path = $"{_soundDirectory}/{newTable.name}.asset";
            string folderPath = $"{_soundDirectory}/{newTable.name}";
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            AssetDatabase.CreateAsset(newTable, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            EditorGUIUtility.PingObject(newTable);
            Selection.activeObject = newTable;
            return newTable;
        }

        private void CreateSoundSO()
        {
            SoundSO soundSO = CreateInstance<SoundSO>();
            Guid guid = Guid.NewGuid();
            soundSO.soundName = $"New_Sound {guid.ToString()}";
            Debug.Log($"[MINI Sound] Sound SO Data Generated. Name:{soundSO.soundName}");
            if (_soundTable.audioType == AudioType.BGM)
            {
                soundSO.audioType = AudioType.BGM;
            }
            else
            {
                soundSO.audioType = AudioType.SFX;
            }
            string path = $"{_soundDirectory}/{_soundTable.name}";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            AssetDatabase.CreateAsset(soundSO, $"{path}/Sound_{soundSO.soundName}.asset");
            _soundTable.soundSOList.Add(soundSO);
            EditorUtility.SetDirty(_soundTable);
            AssetDatabase.SaveAssets();
        }
    }
}
