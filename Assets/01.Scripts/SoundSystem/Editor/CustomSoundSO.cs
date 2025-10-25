using UnityEditor;
using UnityEngine;

namespace MINISoundManage.EditorScripts
{

    [CustomEditor(typeof(SoundSO))]
    public class CustomSoundSO : Editor
    {
        private SerializedProperty nameProp;
        private SerializedProperty typeProp;
        private SerializedProperty clipProp;
        private SerializedProperty loopProp;
        private SerializedProperty randomizeProp;
        private SerializedProperty randomPitchModifierProp;
        private SerializedProperty volumeProp;
        private SerializedProperty pitchProp;

        private void OnEnable()
        {
            GUIUtility.keyboardControl = 0;

            nameProp = serializedObject.FindProperty("soundName");
            typeProp = serializedObject.FindProperty("audioType");
            clipProp = serializedObject.FindProperty("clip");
            loopProp = serializedObject.FindProperty("loop");
            randomizeProp = serializedObject.FindProperty("randomizePitch");
            randomPitchModifierProp = serializedObject.FindProperty("randomPitchModifier");
            volumeProp = serializedObject.FindProperty("volume");
            pitchProp = serializedObject.FindProperty("pitch");
        }

        public override void OnInspectorGUI()
        {

            serializedObject.Update();
            EditorGUILayout.BeginHorizontal("HelpBox");
            {
                EditorGUILayout.BeginVertical();
                {
                    #region EnumName
                    EditorGUI.BeginChangeCheck();
                    string prevName = nameProp.stringValue;
                    EditorGUILayout.LabelField("Basic Setting", EditorStyles.boldLabel);
                    EditorGUILayout.DelayedTextField(nameProp);

                    if (EditorGUI.EndChangeCheck())
                    {
                        // Path of Current Editing Asset.
                        string assetPath = AssetDatabase.GetAssetPath(target);
                        string newName = $"Sound_{nameProp.stringValue}";
                        serializedObject.ApplyModifiedProperties();

                        string msg = AssetDatabase.RenameAsset(assetPath, newName);

                        // Renamed FileName Successfully
                        if (string.IsNullOrEmpty(msg))
                        {
                            target.name = newName;
                            EditorGUILayout.EndVertical();
                            EditorGUILayout.EndHorizontal();
                            return;
                        }

                        nameProp.stringValue = prevName;
                    }
                    #endregion

                    //EditorGUILayout.PropertyField(nameProp);
                    EditorGUILayout.PropertyField(typeProp);
                    EditorGUILayout.PropertyField(clipProp);
                    EditorGUILayout.PropertyField(loopProp);
                    EditorGUILayout.PropertyField(randomizeProp);
                    EditorGUILayout.Slider(randomPitchModifierProp, 0f, 1f);
                    EditorGUILayout.Slider(volumeProp, 0.1f, 2f);
                    EditorGUILayout.Slider(pitchProp, 0.1f, 3f);

                    EditorGUILayout.Space(10f);
                    EditorGUILayout.LabelField("3D Audio Setting", EditorStyles.boldLabel);
                    // TODO : 3D Audio Distance Setting.. etc...

                    // #region Description
                    // EditorGUILayout.BeginVertical("HelpBox");
                    // {
                    //     EditorGUILayout.LabelField("Description");
                    //     
                    //     descriptionProp.stringValue = 
                    //         EditorGUILayout.TextArea(descriptionProp.stringValue,  textAreaStyle, GUILayout.Height(60));
                    // }
                    // EditorGUILayout.EndVertical();
                    // #endregion
                    //
                    // EditorGUILayout.BeginHorizontal();
                    // {
                    //     EditorGUILayout.PrefixLabel("Pool Settings");
                    //     EditorGUILayout.PropertyField(poolCountProp, GUIContent.none);
                    //     EditorGUILayout.PropertyField(prefabProp, GUIContent.none);
                    // }
                    //EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();

            serializedObject.ApplyModifiedProperties();
        }
    }
}